using System;
using System.Collections.Generic;
using System.Text;

namespace BlazingApple.Survey.Shared
{
    public class AnswerResponse
    {
        public string OptionLabel { get; set; }

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

        public List<DTOSurveyItem> SurveyItems { get; set; }

        public string SurveyName { get; set; }

        public string UserId { get; set; }
    }

    /// <summary>DTO for <see cref="SurveyItem" /></summary>
    /// <remarks><seealso cref="SurveyItem" /></remarks>
    public class DTOSurveyItem
    {
        public List<AnswerResponse> AnswerResponses { get; set; }

        public DateTime? AnswerValueDateTime { get; set; }

        public IEnumerable<string> AnswerValueList { get; set; }

        public string AnswerValueString { get; set; }

        /// <inheritdoc cref="SurveyItem.Id" />
        public Guid Id { get; set; }

        /// <inheritdoc cref="SurveyItem.ItemLabel" />
        public string ItemLabel { get; set; }

        /// <inheritdoc cref="SurveyItem.ItemType" />
        public string ItemType { get; set; }

        /// <inheritdoc cref="SurveyItem.ItemValue" />
        public string ItemValue { get; set; }

        /// <inheritdoc cref="SurveyItem.Position" />
        public int Position { get; set; }

        /// <inheritdoc cref="SurveyItem.Required" />
        public bool Required { get; set; }

        public List<DTOSurveyItemOption> SurveyItemOptions { get; set; }
    }

    /// <summary>DTO for <see cref="SurveyItemOption" /></summary>
    /// <remarks><seealso cref="SurveyItemOption" /></remarks>
    public partial class DTOSurveyItemOption
    {
        /// <inheritdoc cref="SurveyItemOption.Id" />
        public Guid Id { get; set; }

        /// <inheritdoc cref="SurveyItemOption.OptionLabel" />
        public string OptionLabel { get; set; }
    }
}
