using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazingApple.Survey.Components.Internal;

/// <summary>Renders a particular <see cref="Shared.Question" /></summary>
public partial class RenderQuestion : ComponentBase
{
    private DateTime? _dummyDateTime;
    private string _dummyString = null!;
    private string _idString = null!;

    private DTOQuestion Answer = new();

    /// <summary>The <see cref="Shared.Question" /> to render.</summary>
    [Parameter, EditorRequired]
    public Question? Question { get; set; }

    /// <inheritdoc />
    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        _idString = "itemControl-" + Question?.Position;
    }
}
