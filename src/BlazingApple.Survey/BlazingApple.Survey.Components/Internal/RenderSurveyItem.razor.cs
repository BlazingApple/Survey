using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazingApple.Survey.Components.Internal;

public partial class RenderSurveyItem : ComponentBase
{
    private DateTime? _dummyDateTime;
    private string _dummyString, _idString;
    private DTOSurveyItem Answer = new DTOSurveyItem();

    /// <summary>The <see cref="Shared.SurveyItem" /> to render.</summary>
    [Parameter, EditorRequired]
    public SurveyItem? Item { get; set; }

    /// <inheritdoc />
    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        _idString = "itemControl-" + Item?.Position;
    }
}
