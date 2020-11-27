using System;
using System.Collections.Generic;
using System.Text;

namespace BlazingApple.Survey.Shared
{
    public partial class SurveyItem { 
        public SurveyItem()
        {
            SurveyAnswers = new HashSet<SurveyAnswer>();
            SurveyItemOptions = new HashSet<SurveyItemOption>();
        }

        public int Id { get; set; }
        public int SurveyId { get; set; }
        public virtual Survey Survey { get; set; }

        public string ItemLabel { get; set; }
        public string ItemType { get; set; }
        public string ItemValue { get; set; }
        public int Position { get; set; }
        public int Required { get; set; }
        public int? SurveyChoiceId { get; set; }

        public virtual ICollection<SurveyAnswer> SurveyAnswers { get; set; }
        public virtual ICollection<SurveyItemOption> SurveyItemOptions { get; set; }
    }
}
