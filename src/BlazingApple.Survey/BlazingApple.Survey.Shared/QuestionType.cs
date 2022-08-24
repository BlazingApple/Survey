using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazingApple.Survey.Shared;

/// <summary>The type of question ( <see cref="Question" />).</summary>
public enum QuestionType
{
    /// <summary>A date picker.</summary>
    Date,

    /// <summary>A date picker question, with the time.</summary>
    DateTime,

    /// <summary>A single-select dropdown.</summary>
    Dropdown,

    /// <summary>A multi-select dropdown.</summary>
    DropdownMultiSelect,

    /// <summary>A large text area allowing for extended response answers.</summary>
    TextArea,

    /// <summary>A short text box, intended for short free-text answers.</summary>
    TextBox,
}
