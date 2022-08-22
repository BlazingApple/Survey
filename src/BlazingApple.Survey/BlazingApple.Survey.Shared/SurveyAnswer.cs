using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BlazingApple.Survey.Shared;

/// <summary>Rep</summary>
public partial class SurveyAnswer
{
    /// <summary>The value of the answer.</summary>
    [Required]
    public string? AnswerValue { get; set; }

    /// <summary>The <see cref="DateTime" /> that the answer was provided.</summary>
    [Required]
    public DateTime? AnswerValueDateTime { get; set; }

    /// <summary>The identifier</summary>
    public Guid Id { get; set; }

    /// <inheritdoc cref="SurveyItem" />
    public virtual SurveyItem? SurveyItem { get; set; }

    /// <summary>FK for <see cref="SurveyItem" /></summary>
    [Required]
    public Guid SurveyItemId { get; set; }
}
