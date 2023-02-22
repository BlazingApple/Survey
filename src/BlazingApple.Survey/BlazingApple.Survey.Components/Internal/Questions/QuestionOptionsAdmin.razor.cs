using Microsoft.AspNetCore.Components;

namespace BlazingApple.Survey.Components.Internal.Questions;

/// <summary>
/// Renders components/controls for editing the options of a dropdown/multi selection quesiton
/// </summary>
public partial class QuestionOptionsAdmin : ComponentBase
{
	/// <inheritdoc cref="Question"/>
	[Parameter, EditorRequired]
	public Question? Question { get; set; }

	private string _newOption = string.Empty;

	private bool _isEditing;

	/// <summary>Remove the option from the list of items.</summary>
	/// <param name="option"></param>
	private void RemoveOption(QuestionOption option)
	{
		if (Question?.Options != null)
		{
			Question.Options.Remove(option);
		}
	}


	/// <summary>Adds a response option to the survey question being edited.</summary>
	private void AddOption()
	{
		if (!string.IsNullOrWhiteSpace(_newOption) && Question?.Options != null)
		{
			Question.Options
				.Add(new QuestionOption
				{
					OptionLabel = _newOption
				});

			_newOption = string.Empty;
			StateHasChanged();
		}
	}

	private void EditToggle()
	{
		_isEditing = !_isEditing;
	}
}
