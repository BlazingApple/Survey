using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BlazingApple.Survey.Shared;

/// <summary>A survey's question.</summary>
public partial class SurveyItem
{
    /// <summary>Id</summary>
    public Guid Id { get; set; }

    public string ItemLabel { get; set; }

    public string ItemType { get; set; }

    public string ItemValue { get; set; }

    /// <summary>The index/position in the list of questions in the <see cref="Survey" /></summary>
    public int Position { get; set; }

    /// <summary>Whether or not this question must be answered.</summary>
    public bool Required { get; set; }

    /// <summary>The survey this question is linked to.</summary>
    public virtual Survey? Survey { get; set; }

    /// <summary>The collection of <see cref="SurveyAnswer" /> tied to this question.</summary>
    public virtual ICollection<SurveyAnswer> SurveyAnswers { get; set; }

    /// <summary>FK for <see cref="Survey" /></summary>
    [Required]
    public Guid SurveyId { get; set; }

    /// <summary>The set of choices/options associated with this question, if any.</summary>
    public virtual ICollection<SurveyItemOption>? SurveyItemOptions { get; set; }

    /// <summary>Default constructor.</summary>
    public SurveyItem()
    {
        SurveyAnswers = new HashSet<SurveyAnswer>();
        SurveyItemOptions = new HashSet<SurveyItemOption>();
    }
}
