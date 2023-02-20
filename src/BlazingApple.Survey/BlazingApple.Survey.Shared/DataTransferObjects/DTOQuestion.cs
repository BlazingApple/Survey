namespace BlazingApple.Survey.Shared.DataTransferObjects;

/// <summary>DTO for <see cref="Question" />, and includes the user's answers.</summary>
public partial class DTOQuestion
{
	/// <inheritdoc cref="AnswerResponse" />
	public List<AnswerResponse>? AnswerResponses { get; set; }

	/// <summary>If the <see cref="Question" /> is of type <see cref="QuestionType.DateTime" />, this represents the user's answer.</summary>
	public DateTime? AnswerValueDateTime { get; set; }

	/// <summary>If the <see cref="Question" /> is of type <see cref="QuestionType.DropdownMultiSelect" />, this represents the user's answer.</summary>
	public IEnumerable<string>? AnswerValueList { get; set; }

	/// <summary>
	///     If the <see cref="Question" /> is of type <see cref="QuestionType.TextArea" /> or <see cref="QuestionType.TextBox" />, this represents the
	///     user's answer.
	/// </summary>
	public string? AnswerValueString { get; set; }

	/// <inheritdoc cref="Question.Id" />
	public Guid Id { get; set; }

	/// <inheritdoc cref="DTOQuestionOption" />
	public List<DTOQuestionOption>? Options { get; set; }

	/// <inheritdoc cref="Question.Position" />
	public int Position { get; set; }

	/// <inheritdoc cref="Question.Prompt" />
	public string? Prompt { get; set; }

	/// <inheritdoc cref="Question.Required" />
	public bool Required { get; set; }

	/// <inheritdoc cref="Question.Type" />
	public QuestionType Type { get; set; }

	/// <summary>
	/// The identifier of the user taking the survey.
	/// </summary>
	public string? UserId { get; set; }
}
