using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BlazingApple.Survey.Shared;

/// <summary>Represents a user that's answered a <see cref="BlazingApple.Survey.Shared.Question" /></summary>
public partial class Answer
{
    /// <summary>The value of the answer.</summary>
    public string? AnswerValue { get; set; }

    /// <summary>The <see cref="DateTime" /> that the answer was provided.</summary>
    public DateTime? AnswerValueDateTime { get; set; }

    /// <summary>The identifier</summary>
    public Guid Id { get; set; }

    /// <inheritdoc cref="Question" />
    public virtual Question? Question { get; set; }

    /// <summary>FK for <see cref="Question" /></summary>
    [Required]
    public Guid QuestionId { get; set; }

    /// <summary>The user id who filled this out (may be anonymous).</summary>
    public string? UserId { get; set; }
}
