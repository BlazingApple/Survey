using System;
using System.Collections.Generic;
using System.Text;

namespace BlazingApple.Survey.Shared
{
    public partial class SurveyItemOption
    {
        public int Id { get; set; }
        public int SurveyItemId { get; set; }
        public virtual SurveyItem SurveyItem { get; set; }
        public string OptionLabel { get; set; }
    }
}
