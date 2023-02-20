using BlazingApple.Survey.Components.Services;
using Microsoft.AspNetCore.Components;
using Radzen;
using System.Diagnostics.CodeAnalysis;

namespace BlazingApple.Survey.Components.Internal;

/// <summary>Render a list of <see cref="Question" /> for a <see cref="Shared.Survey" /></summary>
public partial class SurveyQuestions : ComponentBase
{
	private readonly DialogOptions _options = new() { Width = "550px", Height = "380px" };
	private bool _showNewQuestion;

	/// <inheritdoc cref="Radzen.DialogService" />
	[Parameter]
	public DialogService DialogService { get; set; } = null!;

	/// <summary>If <c>true</c> show the questions for the <see cref="Question" /> inline, otherwise, open a modal popup.</summary>
	[Parameter]
	public bool PromptInline { get; set; }

	/// <summary>The <see cref="Shared.Survey" /> to render questions for.</summary>
	[Parameter, EditorRequired]
	public Shared.Survey? SelectedSurvey { get; set; }

	[Inject]
	private ISurveyClient Service { get; set; } = null!;

	/// <summary>
	/// Pass this if you'd like to override the default route to post the survey to.
	/// </summary>
	[Parameter]
	public string? Route { get; set; }

	[Inject]
	private TooltipService TooltipService { get; set; } = null!;

	/// <summary>Closes the current question, and goes to the next one.</summary>
	private void CloseQuestion()
	{
		_showNewQuestion = false;
		StateHasChanged();
	}

	private void CloseQuestionHandler(object? sender, EventArgs args)
	{
		CloseQuestion();
	}

	private void OpenQuestion()
	{
		if (!PromptInline)
		{
			DialogService.Open<EditQuestion>(
				$"New Question",
				new Dictionary<string, object>() { { nameof(EditQuestion.SelectedQuestion), new Question() { Id = Guid.Empty, Survey = SelectedSurvey } } },
				_options);
		}
		else
		{
			_showNewQuestion = true;
		}
	}

	private async Task RefreshSurvey(Guid SurveyId)
	{
		SelectedSurvey = await @Service.GetSurvey(SurveyId);
	}

	private async Task SelectedSurveyMoveDown(object value)
	{
		Validate();
		Question question = (Question)value;
		int DesiredPosition = question.Position + 1;

		// Move the current element in that position
		Question? currentQuestion = SelectedSurvey.Questions.FirstOrDefault(x => x.Position == DesiredPosition);

		if (currentQuestion != null)
		{
			// Move it up
			currentQuestion.Position--;
			// Update it
			await Service.UpdateQuestion(currentQuestion);
		}

		// Move question Down
		Question QuestionToMoveDown = question;

		if (QuestionToMoveDown != null)
		{
			// Move it up
			QuestionToMoveDown.Position++;
			// Update it
			await Service.UpdateQuestion(QuestionToMoveDown);
		}

		// Refresh SelectedSurvey
		await RefreshSurvey(SelectedSurvey.Id);
	}

	private async Task SelectedSurveyMoveUp(object value)
	{
		Validate();
		Question question = (Question)value;
		int DesiredPosition = question.Position - 1;

		// Move the current element in that position
		Question? currentQuestion = SelectedSurvey.Questions.FirstOrDefault(x => x.Position == DesiredPosition);

		if (currentQuestion != null)
		{
			// Move it down
			currentQuestion.Position++;
			// Update it
			await Service.UpdateQuestion(currentQuestion);
		}

		// Move Item Up
		Question questionToMoveUp = question;

		if (questionToMoveUp != null)
		{
			// Move it up
			questionToMoveUp.Position--;
			// Update it
			await Service.UpdateQuestion(questionToMoveUp);
		}

		// Refresh SelectedSurvey
		SelectedSurvey = await @Service.GetSurvey(SelectedSurvey.Id, Route);
	}

	private void ShowTooltip(ElementReference elementReference, TooltipOptions? options = null)
	{
		TooltipService.Open(elementReference, options?.Text, options);
	}

	[MemberNotNull(nameof(SelectedSurvey))]
	private void Validate()
	{
		if (SelectedSurvey is null)
		{
			throw new ArgumentNullException(nameof(SelectedSurvey), "Invalid object state.");
		}
	}
}
