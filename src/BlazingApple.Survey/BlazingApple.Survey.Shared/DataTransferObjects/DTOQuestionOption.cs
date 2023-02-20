namespace BlazingApple.Survey.Shared.DataTransferObjects;

/// <summary>DTO for <see cref="QuestionOption" /></summary>
public partial class DTOQuestionOption
{
	/// <inheritdoc cref="QuestionOption.Id" />
	public Guid Id { get; set; }

	/// <inheritdoc cref="QuestionOption.OptionLabel" />
	public string? OptionLabel { get; set; }
}
