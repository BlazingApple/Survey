using Microsoft.AspNetCore.Components;
using Radzen;

namespace BlazingApple.Survey.Components.Internal;

/// <summary>Display a single <see cref="Shared.Question" /></summary>
public partial class QuestionDisplay : ComponentBase
{
	private readonly DialogOptions _options = new() { Width = "550px", Height = "380px" };

	private bool _showEditForm;

	/// <inheritdoc cref="Shared.Question" />
	[Parameter, EditorRequired]
	public Question? Question { get; set; }

	/// <summary><c>True</c> if the question should be display inline, <c>false</c> otherwise.</summary>
	[Parameter]
	public bool PromptInline { get; set; }

	private void CloseQuestion(object? sender, EventArgs args)
	{
		_showEditForm = false;
		StateHasChanged();
	}

	private void OpenQuestion()
	{
		if (!PromptInline)
		{
			DialogService.Open<EditQuestion>($"Edit Question", new Dictionary<string, object?>()
			{
				{ nameof(EditQuestion.SelectedQuestion), Question }
			}, _options);
		}
		else
		{
			_showEditForm = true;
		}
	}
}
