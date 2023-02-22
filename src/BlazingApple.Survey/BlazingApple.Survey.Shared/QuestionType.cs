using System.ComponentModel.DataAnnotations;

namespace BlazingApple.Survey.Shared;

/// <summary>The type of question ( <see cref="Question" />).</summary>
public enum QuestionType
{
	/// <summary>A date picker.</summary>
	[Display(Name = "Date")]
	Date,

	/// <summary>A date picker question, with the time.</summary>
	[Display(Name = "Date and Time")]
	DateTime,

	/// <summary>A single-select dropdown.</summary>
	[Display(Name = "Multiple Choice")]
	Dropdown,

	/// <summary>A multi-select dropdown.</summary>
	[Display(Name = "Multiple Choice - Multiple Selection")]
	DropdownMultiSelect,

	/// <summary>A large text area allowing for extended response answers.</summary>
	[Display(Name = "Long Form Response")]
	TextArea,

	/// <summary>A short text box, intended for short free-text answers.</summary>
	[Display(Name = "Short Response")]
	TextBox,
}
