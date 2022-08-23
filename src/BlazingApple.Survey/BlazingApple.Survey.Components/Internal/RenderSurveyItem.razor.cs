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
    private string _dummyString = null!;
    private string _idString = null!;

    private DTOSurveyItem Answer = new();

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
