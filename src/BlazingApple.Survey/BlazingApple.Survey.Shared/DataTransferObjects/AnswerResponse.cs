using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazingApple.Survey.Shared.DataTransferObjects;

/// <summary>Represents a particular question's option value and the number of user's to have selected this option.</summary>
public class AnswerResponse
{
    /// <summary>The answer's value/label.</summary>
    public string OptionLabel { get; set; } = null!;

    /// <summary>The count of responses.</summary>
    public double Responses { get; set; }
}
