﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BlazingApple.Survey.Shared
{
    /// <summary>A multiple choice option for a single survey question <see cref="SurveyItem" />.</summary>
    public partial class SurveyItemOption
    {
        /// <summary>The identifier.</summary>
        public Guid Id { get; set; }

        /// <summary>The display text (and value) for the multiple choice option.</summary>
        [Required]
        public string? OptionLabel { get; set; }

        /// <summary>The question this option is tied to.</summary>
        public virtual SurveyItem? SurveyItem { get; set; }

        /// <summary>Foreign key for <see cref="SurveyItem" /></summary>
        [Required]
        public Guid SurveyItemId { get; set; }
    }
}
