using BlazingApple.Survey.Shared.DataTransferObjects;

namespace BlazingApple.Survey.Shared.Services;

/// <summary>Handles CRUD operations for <see cref="Survey" /></summary>
public partial class SurveyService : ISurveyService
{
	/// <inheritdoc />
	public Task<bool> DeleteQuestion(Guid id)
	{
		throw new NotImplementedException();
	}

	/// <inheritdoc />
	public Task<bool> DeleteSurvey(Guid id)
	{
		throw new NotImplementedException();
	}

	/// <inheritdoc />
	public Task<Survey?> Get(Guid id)
	{
		throw new NotImplementedException();
	}

	/// <inheritdoc />
	public Task<List<Survey>> Get()
	{
		throw new NotImplementedException();
	}

	/// <inheritdoc />
	public Task<List<DTOQuestion>> GetResults(Guid surveyId, LoadArgs loadArgs)
	{
		throw new NotImplementedException();
	}

	/// <inheritdoc />
	public Task<Survey?> Post(Survey survey)
	{
		throw new NotImplementedException();
	}

	/// <inheritdoc />
	public Task PostAnswer(DTOSurvey dtoSurvey)
	{
		throw new NotImplementedException();
	}

	/// <inheritdoc />
	public Task<ResponseOutcome> Put(Guid id, Survey survey)
	{
		throw new NotImplementedException();
	}

	/// <inheritdoc />
	public Task<bool> PutQuestion(Guid id, Question question)
	{
		throw new NotImplementedException();
	}

	/// <inheritdoc />
	public Task<bool> QuestionExists(Guid id)
	{
		throw new NotImplementedException();
	}

	/// <inheritdoc />
	public Task<bool> SurveyAnswerExists(Guid id)
	{
		throw new NotImplementedException();
	}

	/// <inheritdoc />
	public Task<bool> SurveyExists(Guid id)
	{
		throw new NotImplementedException();
	}

	Task<ResponseOutcome> ISurveyService.Put(Guid id, Survey survey)
	{
		throw new NotImplementedException();
	}
}
