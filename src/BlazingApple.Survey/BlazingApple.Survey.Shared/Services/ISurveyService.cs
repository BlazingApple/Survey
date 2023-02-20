using BlazingApple.Survey.Shared.DataTransferObjects;

namespace BlazingApple.Survey.Shared.Services;

/// <summary>
/// CRUD operations for <see cref="BlazingApple.Survey"/> functionality.
/// </summary>
public interface ISurveyService
{
	/// <summary>Get a <see cref="Survey" /></summary>
	/// <param name="id"><see cref="Survey.Id" /></param>
	/// <returns><see cref="Survey" /></returns>
	public Task<Survey?> Get(Guid id);

	/// <summary>Get the list of all <see cref="Survey" /> s</summary>
	/// <returns>A list of all <see cref="Survey" /></returns>
	public Task<List<Survey>> Get();

	/// <summary>Get results for a <see cref="Survey" /></summary>
	/// <param name="surveyId"><see cref="Survey.Id" /></param>
	/// <param name="loadArgs"><see cref="LoadArgs" /></param>
	/// <returns>The list of <see cref="DTOQuestion" /></returns>
	public Task<List<DTOQuestion>> GetResults(Guid surveyId, LoadArgs loadArgs);

	/// <summary>Update an existing <see cref="Survey" /></summary>
	/// <param name="id"><see cref="Survey.Id" /></param>
	/// <param name="survey"><see cref="Survey" /></param>
	/// <returns><see cref="ResponseOutcome" /></returns>
	public Task<ResponseOutcome> Put(Guid id, Survey survey);

	/// <summary>Determines if the current question/option exists.</summary>
	/// <returns><c>true</c> if exists, <c>false</c> otherwise</returns>
	public Task<bool> QuestionExists(Guid id);

	/// <summary>Determines if the survey answer exists.</summary>
	/// <returns><c>true</c> if exists, <c>false</c> otherwise</returns>
	public Task<bool> SurveyAnswerExists(Guid id);

	/// <summary>Determines if the survey Id passed exists.</summary>
	/// <returns><c>true</c> if exists, <c>false</c> otherwise</returns>
	public Task<bool> SurveyExists(Guid id);

	/// <summary>Delete a survey using it's identifier to identify it.</summary>
	/// <param name="id">The survey Identifier.</param>
	/// <returns><c>true</c> if successful deletion, <c>false</c> otherwise</returns>

	public Task<bool> DeleteSurvey(Guid id);

	/// <summary>
	///     Delete's a specific Survey <see cref="Question"/>.
	/// </summary>
	/// <param name="id">The <see cref="Question"/> to delete.</param>
	/// <returns><c>true</c> if success, <c>false, otherwise.</c></returns>
	public Task<bool> DeleteQuestion(Guid id);

	/// <summary>Save a new <see cref="Answer" /> to the database.</summary>
	/// <param name="dtoSurvey"><see cref="DTOSurvey" /></param>
	/// <returns>Async op.</returns>
	public Task PostAnswer(DTOSurvey dtoSurvey);

	/// <summary>Save a new <see cref="Survey" />.</summary>
	/// <param name="survey">The survey to be saved.</param>
	/// <returns>The survey saved, if saved.</returns>
	public Task<Survey?> Post(Survey survey);

	/// <summary>Update a <see cref="Question" /></summary>
	/// <param name="id"><see cref="Question.Id" /></param>
	/// <param name="question"><see cref="Question" /></param>
	/// <returns><c>true</c> if successfully updated, <c>false</c> otherwise.</returns>
	public Task<bool> PutQuestion(Guid id, Question question);
}
