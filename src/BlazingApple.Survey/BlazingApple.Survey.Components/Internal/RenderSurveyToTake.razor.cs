using BlazingApple.Survey.Components.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using System.Diagnostics.CodeAnalysis;

namespace BlazingApple.Survey.Components.Internal;

/// <summary>Allows a consumer to take a survey.</summary>
public partial class RenderSurveyToTake : ComponentBase
{
	private bool SeeResults = false;

	private bool ShowSurveyComplete = false;

	private string strError = "";

	private EditContext? _editContext;
	private ValidationMessageStore? _messageStore;

	[Inject]
	private ISurveyClient Service { get; set; } = null!;

	/// <summary>Whether or not to allow consumers/users to see the results after they complete the survey.</summary>
	[Parameter]
	public bool AllowSeeResults { get; set; } = true;

	/// <summary>The text to show if there are no questions in the survey.</summary>
	[Parameter]
	public string NoQuestionsText { get; set; } = "There aren't any questions to answer!";

	/// <summary>Called when the survey is submitted by the user.</summary>
	[Parameter]
	public EventCallback<DTOSurvey> OnSubmit { get; set; }

	/// <summary>
	/// Pass this if you'd like to override the default route to post the survey to.
	/// </summary>
	[Parameter]
	public string? Route { get; set; }

	/// <summary>The survey to allow the user to take.</summary>
	[Parameter, EditorRequired]
	public DTOSurvey? SelectedSurvey { get; set; }

	/// <summary>What to show when the survey has been completed.</summary>
	[Parameter]
	public string? SurveyCompleteText { get; set; } = "Survey Complete";

	/// <summary>
	/// User taking the survey.
	/// </summary>
	[Parameter, EditorRequired]
	public string UserId { get; set; } = null!;

	private void CompleteSurvey()
	{
		Validate();
		ShowSurveyComplete = true;
	}

	private void OnSeeResultsClick(MouseEventArgs args)
	{
		SeeResults = true;
	}

	private void OnStartOverClick(MouseEventArgs args)
	{
		SeeResults = false;
		ShowSurveyComplete = false;
	}

	/// <inheritdoc />
	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();

		if (SelectedSurvey is null)
			return;

		_editContext = new(SelectedSurvey);
		_messageStore = new(_editContext);
		_editContext.OnValidationRequested += HandleValidationRequested;

	}

	/// <summary>Prevents a lobbying record from being added with 0 hours.</summary>
	/// <param name="sender">The form model/object being validated</param>
	/// <param name="args">Unused.</param>
	private void HandleValidationRequested(object? sender, ValidationRequestedEventArgs args)
	{
		if (_messageStore is null || SelectedSurvey?.Questions is null)
			throw new InvalidOperationException("Unexpected call to unregistered handler");

		_messageStore.Clear();

		Validate();

		int index = -1;
		foreach (DTOQuestion question in SelectedSurvey.Questions)
		{
			index++;
			if (!question.Required)
				continue;
			else if (question.Type is QuestionType.Date or QuestionType.DateTime && (!question.AnswerValueDateTime.HasValue || question.AnswerValueDateTime == DateTime.MinValue))
				_messageStore.Add(() => SelectedSurvey.Questions[index], "Question is required");
			else if (question.Type is QuestionType.DropdownMultiSelect && (question.AnswerValueList is not null && !question.AnswerValueList.Any()))
				_messageStore.Add(() => SelectedSurvey.Questions[index], "Question is required");
			else if (string.IsNullOrEmpty(question.AnswerValueString))
				_messageStore.Add(() => SelectedSurvey.Questions[index], "Question is required");
		}
	}

	/// <summary>Save and submit the survey to the database</summary>
	/// <returns></returns>
	private async Task SaveSurvey()
	{
		Validate();

		try
		{
			bool result = await @Service.TakeSurvey(SelectedSurvey, UserId, Route);

			CompleteSurvey();
		}
		catch (Exception ex)
		{
			strError = ex.GetBaseException().Message;
		}

		if (OnSubmit.HasDelegate)
		{
			await OnSubmit.InvokeAsync(SelectedSurvey!);
		}
	}

	[MemberNotNull(nameof(SelectedSurvey))]
	private void Validate()
	{
		if (SelectedSurvey is null)
		{
			throw new ArgumentNullException(nameof(SelectedSurvey));
		}
	}
}
