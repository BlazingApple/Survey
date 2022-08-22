using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared = BlazingApple.Survey.Shared;

namespace BlazingApple.Survey.Components;

public partial class SurveyAdmin : OwningComponentBase<SurveyService>
{
    private readonly DialogOptions _options = new() { Width = "500px", Height = "280px" };
    private Shared.Survey? _selectedSurvey;
    private List<Shared.Survey>? _surveys;
    private bool? ExistingSurveys = null;
    private bool ShouldShowNewInline, ShouldShowEditInline;

    private string strError = "";

    /// <summary>Edit the survey inline if <c>true</c>, <c>false</c> otherwise.</summary>
    [Parameter]
    public bool PromptInline { get; set; }

    /// <summary>The form title for editing the survey</summary>
    [Parameter]
    public string? Title { get; set; }

    [Inject]
    private DialogService DialogService { get; set; } = null!;

    [Inject]
    private TooltipService TooltipService { get; set; } = null!;

    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        _surveys = new List<Shared.Survey>();
        _selectedSurvey = new Shared.Survey();

        DialogService.OnClose += DialogClose; // detect when a dialog has closed

        _surveys = await @Service.GetAllSurveysAsync();

        if (_surveys.Count > 0)
            _selectedSurvey = _surveys.First();

        ExistingSurveys = _selectedSurvey is not null;
    }

    /// <inheritdoc />
    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        if (string.IsNullOrEmpty(Title))
            Title = "Manage Surveys";
    }

    private void DialogClose(dynamic result)
    {
        Validate();
        if (_selectedSurvey is null)
            throw new InvalidOperationException("Disallowed null for the selected survey");

        if (result != null)
        {
            if (result is SurveyItem modifiedSurveyItem) // A SurveyItem was edited
            {
                // Refresh the SurveyItem
                SurveyItem? existingSurveyItem = _selectedSurvey.SurveyItems.Where(x => x.Id == modifiedSurveyItem.Id).FirstOrDefault();

                if (existingSurveyItem is null)
                    throw new InvalidOperationException("Could not find matching survey item");

                if (modifiedSurveyItem.Id == "-1")
                {
                    // It was deleted
                    _selectedSurvey.SurveyItems.Remove(existingSurveyItem);
                }
                else
                {
                    // Update existing Survey
                    _selectedSurvey.SurveyItems.Remove(existingSurveyItem);
                    _selectedSurvey.SurveyItems.Add(modifiedSurveyItem);
                }

                StateHasChanged();
                return;
            }
            else if (result is Shared.Survey modifiedSurvey) // A Survey was Edited
            {
                // See if Survey is already in the list
                Shared.Survey? surveyToEdit = _surveys.Where(x => x.Id == modifiedSurvey.Id).FirstOrDefault();

                // Survey does not exist - Add it
                if (surveyToEdit is null)
                {
                    _surveys.Add(modifiedSurvey);
                }
                else
                {
                    // Update existing Survey
                    _surveys.Remove(surveyToEdit);
                    _surveys.Add(modifiedSurvey);
                    return;
                }

                ExistingSurveys = true;
                _selectedSurvey = _surveys.Where(x => x.Id == modifiedSurvey.Id).FirstOrDefault();
                StateHasChanged();
            }
            else if (result is string surveyDeletedId)
            {
                // A Survey was deleted
                if (!string.IsNullOrEmpty(surveyDeletedId))
                {
                    Shared.Survey? surveyToDelete = _surveys.Where(x => x.Id == surveyDeletedId).FirstOrDefault();

                    if (surveyToDelete != null)
                        _surveys.Remove(surveyToDelete);

                    if (_surveys.Count > 0)
                    {
                        ExistingSurveys = true;
                        _selectedSurvey = _surveys.FirstOrDefault();
                    }
                    else
                    {
                        ExistingSurveys = false;
                        _selectedSurvey = null;
                    }

                    StateHasChanged();
                }
            }
        }
    }

    private void OnEditSurveyClick(MouseEventArgs args)
    {
        if (PromptInline)
        {
            ShouldShowEditInline = !ShouldShowEditInline;
            if (ShouldShowEditInline)
                ShouldShowNewInline = false;
        }
        else
        {
            DialogService.Open<EditSurvey>($"New Survey",
            new Dictionary<string, object>() { { "SelectedSurvey", _selectedSurvey } },
            _options);
        }
    }

    private void OnNewSurveyClick(MouseEventArgs args)
    {
        if (PromptInline)
        {
            ShouldShowNewInline = !ShouldShowNewInline;

            if (ShouldShowNewInline)
                ShouldShowEditInline = false;
        }
        else
        {
            DialogService.Open<EditSurvey>($"New Survey",
            new Dictionary<string, object>() {
                { "SelectedSurvey", new Shared.Survey() { Id = Guid.Empty } } },
            _options);
        }
    }

    private async Task RefreshSurveys(Guid surveyToRefresh)
    {
        _surveys = await Service.GetAllSurveysAsync();
        _selectedSurvey = _surveys.Where(x => x.Id == surveyToRefresh).FirstOrDefault();
    }

    private async Task SelectedSurveyChange(object value)
    {
        if (_selectedSurvey == null)
            return;

        await RefreshSurveys(_selectedSurvey.Id);
        StateHasChanged();
    }

    private void ShowTooltip(ElementReference elementReference, TooltipOptions? options = null)
        => TooltipService.Open(elementReference, options?.Text, options);

    [MemberNotNull(nameof(_surveys))]
    private void Validate()
    {
        if (_surveys is null)
            throw new ArgumentNullException(nameof(_surveys), "Invalid null.");
    }
}
