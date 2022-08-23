using BlazingApple.Survey.Shared;
using Microsoft.AspNetCore.Components;
using Radzen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazingApple.Survey;
using System.Diagnostics.CodeAnalysis;

namespace BlazingApple.Survey.Components.Internal;

/// <summary>Render a list of <see cref="SurveyItem" /> for a <see cref="Shared.Survey" /></summary>
public partial class SurveyItems : ComponentBase
{
    private readonly DialogOptions _options = new() { Width = "550px", Height = "380px" };
    private bool _showNewQuestion;

    /// <inheritdoc cref="Radzen.DialogService" />
    [Parameter]
    public DialogService DialogService { get; set; } = null!;

    /// <summary>If <c>true</c> show the questions for the <see cref="SurveyItem" /> inline, otherwise, open a modal popup.</summary>
    [Parameter]
    public bool PromptInline { get; set; }

    /// <summary>The <see cref="Shared.Survey" /> to render questions for.</summary>
    [Parameter, EditorRequired]
    public Shared.Survey? SelectedSurvey { get; set; }

    [Inject]
    private SurveyService Service { get; set; } = null!;

    [Inject]
    private TooltipService TooltipService { get; set; } = null!;

    /// <summary>Closes the current question, and goes to the next one.</summary>
    private void CloseQuestion()
    {
        _showNewQuestion = false;
        StateHasChanged();
    }

    private void CloseQuestionHandler(object? sender, EventArgs args)
        => CloseQuestion();

    private void OpenQuestion()
    {
        if (!PromptInline)
        {
            DialogService.Open<EditSurveyItem>(
                $"New Question",
                new Dictionary<string, object>() { { "SelectedSurveyItem", new SurveyItem() { Id = Guid.Empty, Survey = SelectedSurvey } } },
                _options);
        }
        else
        {
            _showNewQuestion = true;
        }
    }

    private async Task RefreshSurvey(Guid SurveyId)
        => SelectedSurvey = await @Service.GetSurvey(SurveyId);

    private async Task SelectedSurveyMoveDown(object value)
    {
        Validate();
        SurveyItem objSurveyItem = (SurveyItem)value;
        int DesiredPosition = (objSurveyItem.Position + 1);

        // Move the current element in that position
        var CurrentSurveyItem = SelectedSurvey.SurveyItems.FirstOrDefault(x => x.Position == DesiredPosition);

        if (CurrentSurveyItem != null)
        {
            // Move it up
            CurrentSurveyItem.Position--;
            // Update it
            await Service.UpdateSurveyItemAsync(CurrentSurveyItem);
        }

        // Move Item Down
        SurveyItem SurveyItemToMoveDown = objSurveyItem;

        if (SurveyItemToMoveDown != null)
        {
            // Move it up
            SurveyItemToMoveDown.Position++;
            // Update it
            await Service.UpdateSurveyItemAsync(SurveyItemToMoveDown);
        }

        // Refresh SelectedSurvey
        await RefreshSurvey(SelectedSurvey.Id);
    }

    private async Task SelectedSurveyMoveUp(object value)
    {
        Validate();
        SurveyItem objSurveyItem = (SurveyItem)value;
        int DesiredPosition = (objSurveyItem.Position - 1);

        // Move the current element in that position
        var CurrentSurveyItem = SelectedSurvey.SurveyItems.FirstOrDefault(x => x.Position == DesiredPosition);

        if (CurrentSurveyItem != null)
        {
            // Move it down
            CurrentSurveyItem.Position++;
            // Update it
            await Service.UpdateSurveyItemAsync(CurrentSurveyItem);
        }

        // Move Item Up
        SurveyItem SurveyItemToMoveUp = objSurveyItem;

        if (SurveyItemToMoveUp != null)
        {
            // Move it up
            SurveyItemToMoveUp.Position--;
            // Update it
            await Service.UpdateSurveyItemAsync(SurveyItemToMoveUp);
        }

        // Refresh SelectedSurvey
        SelectedSurvey = await @Service.GetSurvey(SelectedSurvey.Id);
    }

    private void ShowTooltip(ElementReference elementReference, TooltipOptions? options = null)
        => TooltipService.Open(elementReference, options?.Text, options);

    [MemberNotNull(nameof(SelectedSurvey))]
    private void Validate()
    {
        if (SelectedSurvey is null)
            throw new ArgumentNullException(nameof(SelectedSurvey), "Invalid object state.");
    }
}
