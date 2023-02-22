using System.Net.Http.Json;

namespace BlazingApple.Survey.Components.Services;

/// <summary>Handles I/O for <see cref="Shared.Survey" /></summary>
public class SurveyClient : ISurveyClient
{
	private static readonly string API_PREFIX = "api/surveys";
	private readonly HttpClient _client;

	/// <summary>DI Constructor.</summary>
	public SurveyClient(HttpClient client)
	{
		_client = client;
	}

	/// <inheritdoc/>
	public DTOSurvey ConvertSurveyToDTO(Shared.Survey survey)
	{
		DTOSurvey dtoSurvey = new()
		{
			Id = survey.Id,
			Name = survey.Name,
			Questions = new List<DTOQuestion>(),
		};

		foreach (Question question in survey.Questions)
		{
			DTOQuestion dtoQuestion = new()
			{
				Id = question.Id,
				Prompt = question.Prompt,
				Type = question.Type,
				Position = question.Position,
				Required = question.Required,

				Options = new List<DTOQuestionOption>()
			};

			foreach (QuestionOption option in question.Options!.OrderBy(x => x.Id))
			{
				DTOQuestionOption questionOption = new()
				{
					Id = option.Id,
					OptionLabel = option.OptionLabel,
				};

				dtoQuestion.Options.Add(questionOption);
			}

			dtoSurvey.Questions.Add(dtoQuestion);
		}
		return dtoSurvey;
	}

	/// <inheritdoc/>
	public async Task<bool> TakeSurvey(DTOSurvey surveyWithAnswers, string? userId = null, string? routeOverride = null)
	{
		if (surveyWithAnswers.Questions is null)
		{
			return false;
		}

		foreach (DTOQuestion questionAnswer in surveyWithAnswers.Questions)
		{
			questionAnswer.UserId = userId;
		}

		string route = routeOverride ?? API_PREFIX + "/answers";
		HttpResponseMessage response = await _client.PostAsJsonAsync(route, surveyWithAnswers);
		return response.IsSuccessStatusCode;
	}

	/// <inheritdoc/>
	public async Task<Shared.Survey> CreateSurveyAsync(Shared.Survey newSurvey, string? additionalSegments = null)
	{
		newSurvey.Id = Guid.Empty;
		newSurvey.DateCreated = DateTime.Now;
		HttpResponseMessage response = await _client.PostAsJsonAsync(string.Join('/', API_PREFIX, additionalSegments), newSurvey);
		response.EnsureSuccessStatusCode();

		Shared.Survey? result = await response.Content.ReadFromJsonAsync<Shared.Survey>();
		return result!;
	}


	/// <inheritdoc/>
	public async Task<Question> CreateQuestionAsync(Question newQuestion)
	{
		newQuestion.SurveyId = newQuestion.Survey!.Id;
		newQuestion.Position = newQuestion.Survey.Questions.Count;
		Question dtoQuestion = GetDtoQuestionToCopy(newQuestion);

		HttpResponseMessage response = await _client.PostAsJsonAsync(API_PREFIX + "/questions", dtoQuestion);
		response.EnsureSuccessStatusCode();

		return (await response.Content.ReadFromJsonAsync<Question>())!;
	}


	/// <inheritdoc/>
	public async Task<bool> DeleteSurveyAsync(Shared.Survey existingSurvey)
	{
		try
		{
			HttpResponseMessage response = await _client.DeleteAsync(API_PREFIX + "/" + existingSurvey.Id);
			response.EnsureSuccessStatusCode();
		}
		catch
		{
			return false;
		}
		return true;
	}

	/// <inheritdoc/>
	public async Task<bool> DeleteQuestionAsync(Question question)
	{
		try
		{
			HttpResponseMessage response = await _client.DeleteAsync($"{API_PREFIX}/questions/{question.Id}");
			response.EnsureSuccessStatusCode();
		}
		catch
		{
			return false;
		}

		return true;
	}


	/// <inheritdoc/>
	public async Task<List<Question>> GetAllQuestionsAsync(string surveyId)
	{
		return (await _client.GetFromJsonAsync<List<Question>>($"{API_PREFIX}/{surveyId}/questions"))!;
	}

	/// <inheritdoc/>
	public async Task<List<Shared.Survey>> GetAllSurveysAsync(string? routeOverride = null)
	{
		string route = routeOverride ?? API_PREFIX;
		List<Shared.Survey> surveys = (await _client.GetFromJsonAsync<List<Shared.Survey>>(route))!;

		return surveys.OrderBy(x => x.Name).ToList();
	}

	/// <inheritdoc/>
	public async Task<Shared.Survey> GetSurvey(Guid Id, string? routeOverride = null)
	{
		string route = routeOverride ?? $"{API_PREFIX}/{Id}";
		Shared.Survey? response = await _client.GetFromJsonAsync<Shared.Survey>(route);
		return response is null ? throw new InvalidDataException("Bad response from the server") : response;
	}


	/// <inheritdoc/>
	public async Task<Question> GetQuestion(string questionId, string? queryParamAndValue = null)
	{
		string route = $"{API_PREFIX}/questions/{questionId}";
		if (queryParamAndValue != null)
		{
			route += $"?{queryParamAndValue}";
		}

		return (await _client.GetFromJsonAsync<Question>(route))!;
	}

	/// <inheritdoc/>
	public async Task<List<DTOQuestion>> GetSurveyResults(Guid surveyId, LoadArgs args, string? routeOverride = null)
	{
		string route = routeOverride ?? $"{API_PREFIX}/results/{surveyId}";
		HttpResponseMessage response = await _client.PostAsJsonAsync(route, args);
		response.EnsureSuccessStatusCode();
		List<DTOQuestion> result = (await response.Content.ReadFromJsonAsync<List<DTOQuestion>>())!;
		return result;
	}

	/// <inheritdoc/>
	public async Task<int> GetSurveyResultsCount(Guid surveyId, string? routeOverride = null)
	{
		string route = routeOverride ?? $"{API_PREFIX}/results/{surveyId}/count";
		return await _client.GetFromJsonAsync<int>(route);
	}


	/// <inheritdoc/>
	public async Task<Shared.Survey> UpdateSurveyAsync(Shared.Survey existingSurvey)
	{
		HttpResponseMessage response = await _client.PutAsJsonAsync($"{API_PREFIX}/{existingSurvey.Id}", existingSurvey);
		response.EnsureSuccessStatusCode();

		return (await response.Content.ReadFromJsonAsync<Shared.Survey>())!;
	}


	/// <inheritdoc/>
	public async Task<Question> UpdateQuestion(Question existingQuestion)
	{
		Question dtoQuestion = GetDtoQuestionToCopy(existingQuestion);

		HttpResponseMessage response = await _client.PutAsJsonAsync(API_PREFIX + "/questions/" + dtoQuestion.Id, dtoQuestion);
		response.EnsureSuccessStatusCode();
		Question? result = await response.Content.ReadFromJsonAsync<Question>();
		return result!;
	}

	// Survey Answers
	private Question GetDtoQuestionToCopy(Question copy)
	{
		Question question = new()
		{
			Id = copy.Id,
			SurveyId = copy.SurveyId,
			Position = copy.Position,
			Prompt = copy.Prompt,
			Type = copy.Type,
			Required = copy.Required,
			Options = copy.Options
		};
		return question;
	}
}
