using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazingApple.Survey.Components.Internal;

/// <summary>Render the result for a single question.</summary>
public partial class RenderSurveyResult : ComponentBase
{
    /// <summary>The name of the question or <see cref="SurveyItem" /></summary>
    [Parameter, EditorRequired]
    public string Label { get; set; } = null!;

    /// <summary>The responses received.</summary>
    [Parameter, EditorRequired]
    public List<AnswerResponse>? Responses { get; set; }
}
