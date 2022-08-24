using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BlazingApple.Survey.Shared;

/// <summary>A survey's question.</summary>
public partial class Question
{
    /// <summary>The collection of <see cref="Answer" /> tied to this question.</summary>
    public virtual ICollection<Answer> Answers { get; set; }

    /// <summary>Id</summary>
    public Guid Id { get; set; }

    /// <summary>The set of choices/options associated with this question, if any.</summary>
    public virtual ICollection<QuestionOption>? Options { get; set; }

    /// <summary>The index/position in the list of questions in the <see cref="Survey" /></summary>
    public int Position { get; set; }

    /// <summary>Prompt/label of the question.</summary>
    public string? Prompt { get; set; }

    /// <summary>Whether or not this question must be answered.</summary>
    public bool Required { get; set; }

    /// <summary>The survey this question is linked to.</summary>
    public virtual Survey? Survey { get; set; }

    /// <summary>FK for <see cref="Survey" /></summary>
    [Required]
    public Guid SurveyId { get; set; }

    /// <inheritdoc cref="Shared.QuestionType" />
    public QuestionType Type { get; set; }

    /// <summary>Default constructor.</summary>
    public Question()
    {
        Answers = new HashSet<Answer>();
        Options = new HashSet<QuestionOption>();
    }
}
