using System;
using System.Collections.Generic;
using System.Text;

namespace BlazingApple.Survey.Shared
{
    public partial class SurveyAnswer
    {
        public int Id { get; set; }
        public int SurveyItemId { get; set; }
        public string AnswerValue { get; set; }
        public DateTime? AnswerValueDateTime { get; set; }
        public string UserId { get; set; }

        public virtual SurveyItem SurveyItem { get; set; }
    }
}
