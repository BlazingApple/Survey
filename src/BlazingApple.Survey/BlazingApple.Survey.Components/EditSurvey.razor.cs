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

    /// <summary>The survey that the user is editing.</summary>
    [Parameter, EditorRequired]
    public Shared.Survey? SelectedSurvey { get; set; }

    [Inject]
    private DialogService DialogService { get; set; } = null!;

    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
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
