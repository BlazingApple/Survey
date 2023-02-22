namespace BlazingApple.Survey.Components.Services;

/// <summary>
/// <see cref="HttpClient"/> for <see cref="Survey"/> operations.
/// </summary>
public interface ISurveyClient
{
	/// <summary>Translate an entity <see cref="Survey" /> to a <see cref="DTOSurvey" /></summary>
	/// <param name="survey">The entity to translate.</param>
	/// <returns><see cref="DTOSurvey" /></returns>
	DTOSurvey ConvertSurveyToDTO(Shared.Survey survey);

	/// <summary>Take a survey.</summary>
	/// <param name="paramDTOSurvey">The survey being taken.</param>
	/// <param name="userId">The user taking the survey.</param>
	/// <param name="routeOverride">Override the route to post a survey response.</param>
	/// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
	Task<bool> TakeSurvey(DTOSurvey paramDTOSurvey, string? userId = null, string? routeOverride = null);

	/// <summary>Creates a new <see cref="Shared.Survey" /> by POSTing it to the server.</summary>
	/// <param name="newSurvey"><see cref="Shared.Survey" /></param>
	/// <param name="additionalSegments">Additional segments to pass to the controller, if any.</param>
	/// <returns>The survey response from the database.</returns>
	Task<Shared.Survey> CreateSurveyAsync(Shared.Survey newSurvey, string? additionalSegments = null);

	/// <summary>Add a new question to a survey and save it to the database.</summary>
	/// <param name="question">The question to save.</param>
	/// <returns>The saved question.</returns>
	Task<Question> CreateQuestionAsync(Question question);

	/// <summary>Delete a survey.</summary>
	/// <param name="existingSurvey">The survey to delete.</param>
	/// <returns><c>true</c> if deleted, <c>false</c> otherwise.</returns>
	Task<bool> DeleteSurveyAsync(Shared.Survey existingSurvey);

	/// <summary>Delete a question from a survey.</summary>
	/// <param name="question">The question to delete.</param>
	/// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
	Task<bool> DeleteQuestionAsync(Question question);

	/// <summary>Get all of the questions/ <see cref="Question" /> for the survey id.</summary>
	/// <param name="surveyId">The survey identifier.</param>
	/// <returns>The list of questions.</returns>
	Task<List<Question>> GetAllQuestionsAsync(string surveyId);

	/// <summary>Get all the <see cref="Shared.Survey" /> s.</summary>
	/// <param name="routeOverride">The route to retrieve the survey</param>
	/// <returns>The list of <see cref="Shared.Survey" /></returns>
	Task<List<Shared.Survey>> GetAllSurveysAsync(string? routeOverride = null);

	/// <summary>Get a survey.</summary>
	/// <param name="Id">The survey id.</param>
	/// <param name="routeOverride">The route to retrieve the survey</param>
	/// <returns>The survey retrieved from the survey.</returns>
	Task<Shared.Survey> GetSurvey(Guid Id, string? routeOverride = null);

	/// <summary>Get a <see cref="Question" /></summary>
	/// <param name="questionId">The survey question.</param>
	/// <param name="queryParamAndValue">Additional query parameter to add to the get request, if any.</param>
	/// <returns>The question/ <see cref="Question" /></returns>
	Task<Question> GetQuestion(string questionId, string? queryParamAndValue = null);

	/// <summary>Get the results for a particular survey.</summary>
	/// <param name="surveyId">The id for the survey.</param>
	/// <param name="routeOverride">The route override to get the survey results, if any</param>
	/// <param name="args"><see cref="LoadArgs" /></param>
	/// <returns></returns>
	Task<List<DTOQuestion>> GetSurveyResults(Guid surveyId, LoadArgs args, string? routeOverride = null);

	/// <summary>Get the number of completed survey results for a given survey/questionnaire.</summary>
	/// <param name="surveyId">The id of the <see cref="Shared.Survey" /></param>
	/// <param name="routeOverride">The route override to get the survey results, if any</param>
	/// <returns>The count of completed questionnaires.</returns>
	Task<int> GetSurveyResultsCount(Guid surveyId, string? routeOverride = null);

	/// <summary>Update an existing survey.</summary>
	/// <param name="existingSurvey">The survey to updated.</param>
	/// <returns>The survey updated from the database.</returns>
	Task<Shared.Survey> UpdateSurveyAsync(Shared.Survey existingSurvey);

	/// <summary>Update a particular <see cref="Shared.Question" />.</summary>
	/// <param name="existingQuestion">The question to update.</param>
	/// <returns>The object from the database.</returns>
	Task<Question> UpdateQuestion(Question existingQuestion);
}