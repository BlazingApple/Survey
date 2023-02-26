using BlazingApple.Survey.Components.Services;
using Microsoft.AspNetCore.Components;
using Radzen;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace BlazingApple.Survey.Components.Internal.Questions;

/// <summary>Renders an administrative "non-interactive" view of the question.</summary>
public partial class QuestionAdminDisplay : ComponentBase
{
	private bool _isEditing;

	/// <summary>The <see cref="Shared.Question" /> to render.</summary>
	[Parameter, EditorRequired]
	public Question? Question { get; set; }

	/// <summary>The number of questions in the survey.</summary>
	[Parameter, EditorRequired]
	public int QuestionCount { get; set; }

	/// <summary>
	/// The maximum number of <see cref="QuestionOption"/> to render below a question.
	/// </summary>
	[Parameter]
	public int MaxOptionsDisplay { get; set; }
	private void CloseQuestion(object? sender, EventArgs args)
	{
		_isEditing = false;
		StateHasChanged();
	}

	[Inject]
	private DialogService DialogService { get; set; } = null!;

	[Inject]
	private ISurveyClient Service { get; set; } = null!;

	private void OpenQuestion()
	{
		_isEditing = true;
	}

	/// <summary>
	/// The question requested to move up in position.
	/// </summary>
	[Parameter, EditorRequired]
	public EventCallback<Question> MovedUp { get; set; }

	/// <summary>
	/// The question requested to move down in position.
	/// </summary>
	[Parameter, EditorRequired]
	public EventCallback<Question> MovedDown { get; set; }

	/// <summary>
	/// The question is deleted.
	/// </summary>
	[Parameter, EditorRequired]
	public EventCallback<Question> Deleted { get; set; }

	private async Task Delete()
	{
		if (Question == null)
		{
			throw new InvalidOperationException("Disallowed null reference to edited question.");
		}

		bool result = await @Service.DeleteQuestionAsync(Question);

		ItemRequest? request = null;

		if (result)
		{
			request = new(UserAction.Delete, Question, Question.Survey!);
		}

		if (Deleted.HasDelegate)
		{
			await Deleted.InvokeAsync(Question!);
		}

		DialogService.Close(request);
	}

	/// <summary>Using reflection, get the display name of the enum value.</summary>
	/// <param name="value">The enum value.</param>
	/// <returns>The <see cref="DisplayAttribute.Name" /> of the enum value.</returns>
	public static string GetEnumDisplayName(Enum value)
	{
		DisplayAttribute? attribute = GetAttribute<DisplayAttribute>(value);
		return attribute?.Name ?? value.ToString();
	}


	private static TAttrib? GetAttribute<TAttrib>(Enum value)
		where TAttrib : Attribute
	{
		IEnumerable<TAttrib> attributes = GetAttributes<TAttrib>(value);
		return attributes.SingleOrDefault();
	}

	private static IEnumerable<TAttrib> GetAttributes<TAttrib>(Enum value)
		where TAttrib : Attribute
	{
		Type type = value.GetType();
		string name = value.ToString();

		FieldInfo? field = type.GetField(name);
		if (field is null)
		{
			throw new ArgumentException("The provided value was not found", nameof(value));
		}

		IEnumerable<TAttrib> attributes = field.GetCustomAttributes<TAttrib>(false);
		return attributes;
	}

	private string GetQuestionTitleText()
	{
		if (Question is null)
		{
			return "";
		}

		string titleText = Question.Required ? "Required" : "Optional";
		if ((Question.Type is QuestionType.Dropdown or QuestionType.DropdownMultiSelect) && Question.Options is not null)
		{
			titleText += $", {Question.Options.Count} options: {string.Join(',', Question.Options.Select(o => o.OptionLabel))}";
		}

		return titleText;
	}
}