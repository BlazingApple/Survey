using System;
using System.Collections.Generic;
using System.Text;

namespace BlazingApple.Survey.Shared
{
    /// <summary>Represents a particular question's option value and the number of user's to have selected this option.</summary>
    public class AnswerResponse
    {
        /// <summary>The answer's value/label.</summary>
        public string OptionLabel { get; set; } = null!;

        /// <summary>The count of responses.</summary>
        public double Responses { get; set; }
    }

    /// <summary>The data transfer object for <see cref="Survey" /></summary>
    /// <seealso cref="Survey" />
    public class DTOSurvey
    {
        /// <inheritdoc cref="Survey.DateCreated" />
        public DateTime DateCreated { get; set; }

        /// <inheritdoc cref="Survey.DateUpdated" />
        public DateTime? DateUpdated { get; set; }

        /// <summary>The Key</summary>
        public Guid Id { get; set; }

        /// <inheritdoc cref="Survey.Name" />
        public string? Name { get; set; }

        /// <inheritdoc cref="DTOSurveyItem" />
        public List<DTOSurveyItem>? SurveyItems { get; set; }

        /// <inheritdoc cref="Survey.UserId" />
        public string? UserId { get; set; }
    }

    /// <summary>DTO for <see cref="SurveyItem" /></summary>
    /// <remarks><seealso cref="SurveyItem" /></remarks>
    public class DTOSurveyItem
    {
        /// <inheritdoc cref="AnswerResponse" />
        public List<AnswerResponse>? AnswerResponses { get; set; }

        /// <summary>If the <see cref="SurveyItem" /> is of type <see cref="ItemType.DateTime" />, this represents the user's answer.</summary>
        public DateTime? AnswerValueDateTime { get; set; }

        /// <summary>If the <see cref="SurveyItem" /> is of type <see cref="ItemType.DropdownMultiSelect" />, this represents the user's answer.</summary>
        public IEnumerable<string>? AnswerValueList { get; set; }

        /// <summary>
        ///     If the <see cref="SurveyItem" /> is of type <see cref="ItemType.TextArea" /> or <see cref="ItemType.TextBox" />, this represents the
        ///     user's answer.
        /// </summary>
        public string? AnswerValueString { get; set; }

        /// <inheritdoc cref="SurveyItem.Id" />
        public Guid Id { get; set; }

        /// <inheritdoc cref="SurveyItem.ItemType" />
        public string? ItemType { get; set; }

        /// <inheritdoc cref="SurveyItem.ItemValue" />
        public string? ItemValue { get; set; }

        /// <inheritdoc cref="DTOSurveyItemOption" />
        public List<DTOSurveyItemOption>? Options { get; set; }

        /// <inheritdoc cref="SurveyItem.Position" />
        public int Position { get; set; }

        /// <inheritdoc cref="SurveyItem.Prompt" />
        public string? Prompt { get; set; }

        /// <inheritdoc cref="SurveyItem.Required" />
        public bool Required { get; set; }
    }

    /// <summary>DTO for <see cref="SurveyItemOption" /></summary>
    /// <remarks><seealso cref="SurveyItemOption" /></remarks>
    public partial class DTOSurveyItemOption
    {
        /// <inheritdoc cref="SurveyItemOption.Id" />
        public Guid Id { get; set; }

        /// <inheritdoc cref="SurveyItemOption.OptionLabel" />
        public string? OptionLabel { get; set; }
    }
}
