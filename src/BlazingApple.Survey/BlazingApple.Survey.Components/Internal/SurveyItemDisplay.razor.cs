using Microsoft.AspNetCore.Components;
using Radzen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazingApple.Survey.Components.Internal;

/// <summary>Display a single <see cref="Question" /></summary>
public partial class SurveyItemDisplay : ComponentBase
{
    private readonly DialogOptions _options = new() { Width = "550px", Height = "380px" };

    private bool _showEditForm;

    /// <inheritdoc cref="Question" />
    [Parameter, EditorRequired]
    public Question? Item { get; set; }

    /// <summary><c>True</c> if the question should be display inline, <c>false</c> otherwise.</summary>
    [Parameter]
    public bool PromptInline { get; set; }

    private void CloseQuestion(object? sender, EventArgs args)
    {
        _showEditForm = false;
        StateHasChanged();
    }

    private void OpenQuestion()
    {
        if (!PromptInline)
            DialogService.Open<EditSurveyItem>($"Edit Question", new Dictionary<string, object?>() { { "SelectedSurveyItem", Item } }, _options);
        else
            _showEditForm = true;
    }
}
