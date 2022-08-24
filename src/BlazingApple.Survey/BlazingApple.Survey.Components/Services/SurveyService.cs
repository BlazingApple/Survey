using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Radzen;
using shared = BlazingApple.Survey.Shared;

using BlazingApple.Survey.Shared;

using System.Net.Http;
using System.Net.Http.Json;

namespace BlazingApple;

/// <summary>Handles I/O for <see cref="shared.Survey" /></summary>
public class SurveyService
{
    private static string API_PREFIX = "api/surveys";
    private readonly HttpClient _client;

    /// <summary>DI Constructor.</summary>
    public SurveyService(HttpClient client) => _client = client;

    /// <summary>Translate an entity <see cref="shared.Survey" /> to a <see cref="DTOSurvey" /></summary>
    /// <param name="survey">The entity to translate.</param>
    /// <returns><see cref="DTOSurvey" /></returns>
    public DTOSurvey ConvertSurveyToDTO(shared.Survey survey)
    {
        DTOSurvey dtoSurvey = new()
        {
            Id = survey.Id,
            Name = survey.Name,
            Questions = new List<DTOQuestion>(),
        };

        foreach (Question SurveyItem in survey.Questions)
        {
            DTOQuestion dtoSurveyItem = new()
            {
                Id = SurveyItem.Id,
                Prompt = SurveyItem.Prompt,
                Type = SurveyItem.Type,
                Position = SurveyItem.Position,
                Required = SurveyItem.Required,

                Options = new List<DTOQuestionOption>()
            };

            foreach (QuestionOption option in SurveyItem.Options!.OrderBy(x => x.Id))
            {
                DTOQuestionOption objDTOSurveyItemOption = new()
                {
                    Id = option.Id,
                    OptionLabel = option.OptionLabel,
                };

                dtoSurveyItem.Options.Add(objDTOSurveyItemOption);
            }

            dtoSurvey.Questions.Add(dtoSurveyItem);
        }
        return dtoSurvey;
    }

    /// <summary>Add answers/options to a particular <see cref="Question" /> async.</summary>
    /// <param name="paramDTOSurvey">The survey.</param>
    /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
    public async Task<bool> CreateSurveyAnswersAsync(DTOSurvey paramDTOSurvey)
    {
        HttpResponseMessage response = await _client.PostAsJsonAsync(API_PREFIX + "/answers", paramDTOSurvey);
        response.EnsureSuccessStatusCode();
        return true;
    }

    /// <summary>Creates a new <see cref="shared.Survey" /> by POSTing it to the server.</summary>
    /// <param name="newSurvey"><see cref="shared.Survey" /></param>
    /// <returns>The survey response from the database.</returns>
    public async Task<shared.Survey> CreateSurveyAsync(shared.Survey newSurvey)
    {
        newSurvey.Id = Guid.Empty;
        newSurvey.DateCreated = DateTime.Now;
        HttpResponseMessage response = await _client.PostAsJsonAsync(API_PREFIX, newSurvey);
        response.EnsureSuccessStatusCode();

        shared.Survey? result = await response.Content.ReadFromJsonAsync<shared.Survey>();
        return result!;
    }

    /// <summary>Add a new question to a survey and save it to the database.</summary>
    /// <param name="newSurveyItem">The question to save.</param>
    /// <returns>The saved question.</returns>
    public async Task<Question> CreateSurveyItemAsync(Question newSurveyItem)
    {
        newSurveyItem.SurveyId = newSurveyItem.Survey!.Id;
        newSurveyItem.Position = newSurveyItem.Survey.Questions.Count;
        Question dtoSurveyItem = GetDTOItemCopy(newSurveyItem);

        HttpResponseMessage response = await _client.PostAsJsonAsync(API_PREFIX + "/items", dtoSurveyItem);
        response.EnsureSuccessStatusCode();

        return (await response.Content.ReadFromJsonAsync<Question>())!;
    }

    /// <summary>Delete a survey.</summary>
    /// <param name="existingSurvey">The survey to delete.</param>
    /// <returns><c>true</c> if deleted, <c>false</c> otherwise.</returns>
    public async Task<bool> DeleteSurveyAsync(shared.Survey existingSurvey)
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

    /// <summary>Delete a question from a survey.</summary>
    /// <param name="item">The question to delete.</param>
    /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
    public async Task<bool> DeleteSurveyItemAsync(Question item)
    {
        try
        {
            HttpResponseMessage response = await _client.DeleteAsync($"{API_PREFIX}/items/{item.Id}");
            response.EnsureSuccessStatusCode();
        }
        catch
        {
            return false;
        }

        return true;
    }

    /// <summary>Get all of the questions/ <see cref="Question" /> for the survey id.</summary>
    /// <param name="surveyId">The survey identifier.</param>
    /// <returns>The list of questions.</returns>
    public async Task<List<Question>> GetAllSurveyItemsAsync(string surveyId)
        => (await _client.GetFromJsonAsync<List<Question>>($"{API_PREFIX}/{surveyId}/items"))!;

    /// <summary>Get all the <see cref="shared.Survey" /> s.</summary>
    /// <returns>The list of <see cref="shared.Survey" /></returns>
    public async Task<List<shared.Survey>> GetAllSurveysAsync()
    {
        List<shared.Survey> surveys = (await _client.GetFromJsonAsync<List<shared.Survey>>(API_PREFIX))!;

        return surveys.OrderBy(x => x.Name).ToList();
    }

    /// <summary>Get a survey.</summary>
    /// <param name="Id">The survey id.</param>
    /// <returns>The survey retrieved from the survey.</returns>
    public async Task<shared.Survey> GetSurvey(Guid Id)
    {
        shared.Survey? response = await _client.GetFromJsonAsync<shared.Survey>(API_PREFIX + "/" + Id);
        if (response is null)
            throw new InvalidDataException("Bad response from the server");

        return response;
    }

    /// <summary>Get a <see cref="Question" /></summary>
    /// <param name="surveyItemId">The survey question.</param>
    /// <returns>The question/ <see cref="Question" /></returns>
    public async Task<Question> GetSurveyItemAsync(string surveyItemId)
        => (await _client.GetFromJsonAsync<Question>($"{API_PREFIX}/items/{surveyItemId}"))!;

    /// <summary>Get the results for a particular survey.</summary>
    /// <param name="surveyId">The id for the survey.</param>
    /// <param name="args"><see cref="LoadDataArgs" /></param>
    /// <returns></returns>
    public async Task<List<DTOQuestion>> GetSurveyResults(Guid surveyId, LoadDataArgs args)
    {
        HttpResponseMessage response = await _client.PostAsJsonAsync($"{API_PREFIX}/results/{surveyId}", args);
        response.EnsureSuccessStatusCode();
        List<DTOQuestion> result = (await response.Content.ReadFromJsonAsync<List<DTOQuestion>>())!;
        return result;
    }

    /// <summary>Get the number of completed survey results for a given survey/questionnaire.</summary>
    /// <param name="surveyId">The id of the <see cref="shared.Survey" /></param>
    /// <returns>The count of completed questionnaires.</returns>
    public async Task<int> GetSurveyResultsCount(Guid surveyId)
        => await _client.GetFromJsonAsync<int>($"{API_PREFIX}/results/{surveyId}/count");

    /// <summary>Update an existing survey.</summary>
    /// <param name="existingSurvey">The survey to updated.</param>
    /// <returns>The survey updated from the database.</returns>
    public async Task<shared.Survey> UpdateSurveyAsync(shared.Survey existingSurvey)
    {
        HttpResponseMessage response = await _client.PutAsJsonAsync($"{API_PREFIX}/{existingSurvey.Id}", existingSurvey);
        response.EnsureSuccessStatusCode();

        return (await response.Content.ReadFromJsonAsync<shared.Survey>())!;
    }

    /// <summary>Update a particular <see cref="shared.Question" />.</summary>
    /// <param name="objExistingSurveyItem">The question to update.</param>
    /// <returns>The object from the database.</returns>
    public async Task<Question> UpdateSurveyItemAsync(Question objExistingSurveyItem)
    {
        Question dtoSurveyItem = GetDTOItemCopy(objExistingSurveyItem);

        HttpResponseMessage response = await _client.PutAsJsonAsync(API_PREFIX + "/items/" + dtoSurveyItem.Id, dtoSurveyItem);
        response.EnsureSuccessStatusCode();
        Question? result = await response.Content.ReadFromJsonAsync<Question>();
        return result!;
    }

    // Survey Answers
    private Question GetDTOItemCopy(Question itemToCopy)
    {
        Question dtoSurveyItem = new();
        dtoSurveyItem.Id = itemToCopy.Id;
        dtoSurveyItem.SurveyId = itemToCopy.SurveyId;
        dtoSurveyItem.Position = itemToCopy.Position;
        dtoSurveyItem.Prompt = itemToCopy.Prompt;
        dtoSurveyItem.Type = itemToCopy.Type;
        dtoSurveyItem.Required = itemToCopy.Required;
        dtoSurveyItem.Options = itemToCopy.Options;
        return dtoSurveyItem;
    }
}
