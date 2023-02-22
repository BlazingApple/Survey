using BlazingApple.Survey.Components.Services;
using Microsoft.AspNetCore.Components;

namespace BlazingApple.Survey.Components.Internal.Questions;

/// <summary>Allows editing a <see cref="Question" /></summary>
public partial class EditQuestion : ComponentBase
{
	private readonly IEnumerable<QuestionType> FormTypes = Enum.GetValues<QuestionType>();
	private string strError = "";

	/// <summary>Triggered when the edit option is closed.</summary>
	[Parameter]
	public EventHandler? OnClose { get; set; }

	/// <summary>
	/// Invoked when the question has been saved/changed in someway.
	/// </summary>
	[Parameter]
	public EventCallback OnChange { get; set; }

	[Inject]
	private ISurveyClient Service { get; set; } = null!;

	/// <summary>The question being edited.</summary>
	[Parameter, EditorRequired]
	public Question? SelectedQuestion { get; set; }

	private async Task AddOrUpdate()
	{
		if (SelectedQuestion == null)
		{
			return;
		}

		try
		{
			ItemRequest request;
			if (SelectedQuestion.Id == default)
			{
				SelectedQuestion = await Service.CreateQuestionAsync(SelectedQuestion);
				request = new ItemRequest(UserAction.Create, SelectedQuestion, SelectedQuestion.Survey!);
			}
			else
			{
				SelectedQuestion = await Service.UpdateQuestion(SelectedQuestion);
				request = new ItemRequest(UserAction.Update, SelectedQuestion, SelectedQuestion.Survey!);
			}

			dialogService.Close(request);
			await CloseQuestion(false);
		}
		catch (Exception ex)
		{
			strError = ex.GetBaseException().Message;
		}
	}

	private async Task CloseQuestion(bool closeDialog = true)
	{
		OnClose?.Invoke(this, EventArgs.Empty);

		if (closeDialog)
		{
			dialogService.Close();
		}

		if (OnChange.HasDelegate)
		{
			await OnChange.InvokeAsync();
		}
	}
}
