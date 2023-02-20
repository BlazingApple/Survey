using BlazingApple.Survey.Components.Services;
using Microsoft.AspNetCore.Components;
using Radzen;
using System.Diagnostics.CodeAnalysis;

namespace BlazingApple.Survey.Components;

/// <summary>Edit an existing survey.</summary>
public partial class EditSurvey
{
    private string strError = "";

    /// <summary>Called after the user finishes editing.</summary>
    [Parameter]
    public EventHandler? OnClose { get; set; }

    /// <summary>
    /// Additional route segments when posting or editing a survey
    /// </summary>
    [Parameter]
    public string? AdditionalSegments { get; set; }

    [Inject]
    private ISurveyClient Service { get; set; } = null!;

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
        SelectedSurvey ??= new Shared.Survey();
    }

    private async Task AddOrUpdate()
    {
        Validate();
        try
        {
            SurveyRequest response;
            if (SelectedSurvey.Id == default)
            {
                SelectedSurvey = await @Service.CreateSurveyAsync(SelectedSurvey, AdditionalSegments);
                response = new(UserAction.Create, SelectedSurvey);
            }
            else
            {
                SelectedSurvey = await @Service.UpdateSurveyAsync(SelectedSurvey);
                response = new(UserAction.Update, SelectedSurvey);
            }

            DialogService.Close(response);
            InvokeOnClose();
        }
        catch (Exception ex)
        {
            strError = ex.GetBaseException().Message;
        }
    }

    private void Cancel()
    {
        DialogService.Close();
        InvokeOnClose();
    }

    private async Task Delete()
    {
        Validate();
        bool result = await @Service.DeleteSurveyAsync(SelectedSurvey);

        if (!result)
        {
            throw new InvalidDataException("Error deleting survey");
        }

        SurveyRequest response = new(UserAction.Delete, SelectedSurvey);

        DialogService.Close(response);
        InvokeOnClose();
    }

    private void InvokeOnClose()
    {
        OnClose?.Invoke(this, EventArgs.Empty);
    }

    [MemberNotNull(nameof(SelectedSurvey))]
    private void Validate()
    {
        if (SelectedSurvey is null)
        {
            throw new InvalidOperationException();
        }
    }
}
