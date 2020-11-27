using System;
using System.Collections.Generic;

namespace BlazingApple.Survey.Shared
{
    public partial class Survey
    {
        public Survey()
        {
            SurveyItems = new HashSet<SurveyItem>();
        }

        public int Id { get; set; }
        public string SurveyName { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string UserId { get; set; }

        public virtual ICollection<SurveyItem> SurveyItems { get; set; }
    }
}
