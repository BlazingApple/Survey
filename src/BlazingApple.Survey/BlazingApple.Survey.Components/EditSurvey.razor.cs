using Microsoft.AspNetCore.Components;
using Radzen;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazingApple.Survey.Components;

/// <summary>Edit an existing survey.</summary>
public partial class EditSurvey : OwningComponentBase<SurveyService>
{
    private string strError = "";

    /// <summary>Called after the user finishes editing.</summary>
    [Parameter]
    public EventHandler? OnClose { get; set; }

    /// <summary>Whether to launch the edit pane in-line or as a modal.</summary>
    [Parameter]
    public bool PromptInline { get; set; }

    /// <summary>The survey that the user is editing.</summary>
    [Parameter, EditorRequired]
    public Shared.Survey? SelectedSurvey { get; set; }

    [Inject]
    private DialogService DialogService { get; set; } = null!;

    /// <inheritdoc />
    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();
        if (SelectedSurvey == null)
            SelectedSurvey = new Shared.Survey();
    }

    private void Cancel()
    {
        DialogService.Close();
        InvokeOnClose();
    }

    private async Task DeleteSurvey()
    {
        Validate();
        bool result = await @Service.DeleteSurveyAsync(SelectedSurvey);

        DialogService.Close(SelectedSurvey.Id);
        InvokeOnClose();
    }

    private void InvokeOnClose()
    {
        if (OnClose != null)
            OnClose.Invoke(this, null);
    }

    private async Task UpdateSurvey()
    {
        Validate();
        try
        {
            if (SelectedSurvey.Id == default)
                SelectedSurvey = await @Service.CreateSurveyAsync(SelectedSurvey);
            else
                SelectedSurvey = await @Service.UpdateSurveyAsync(SelectedSurvey);

            DialogService.Close(SelectedSurvey);
            InvokeOnClose();
        }
        catch (Exception ex)
        {
            strError = ex.GetBaseException().Message;
        }
    }

    [MemberNotNull(nameof(SelectedSurvey))]
    private void Validate()
    {
        if (SelectedSurvey is null)
            throw new InvalidOperationException();
    }
}
