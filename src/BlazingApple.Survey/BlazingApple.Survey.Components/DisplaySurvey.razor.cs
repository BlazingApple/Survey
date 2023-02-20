using BlazingApple.Survey.Components.Services;
using Microsoft.AspNetCore.Components;
using Radzen;
using System.Diagnostics.CodeAnalysis;

namespace BlazingApple.Survey.Components;

/// <summary>Displays a survey for an end user.</summary>
public partial class DisplaySurvey : ComponentBase
{
    private bool? _existingSurveys;
    private DTOSurvey? _selectedSurvey;

    private List<DTOSurvey>? _surveys;
    private string? strError;

    [Inject]
    private ISurveyClient Service { get; set; } = null!;

    /// <summary>
    /// User taking the survey.
    /// </summary>
    [Parameter, EditorRequired]
    public string UserId { get; set; } = null!;


    /// <summary>Load survey results.</summary>
    /// <returns>Async op.</returns>
    public async Task LoadSurveyResultsData()
    {
        if (_selectedSurvey is null)
        {
            throw new ArgumentNullException(nameof(_selectedSurvey), "Disallowed null.");
        }

        await InvokeAsync(StateHasChanged);
    }

    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        strError = "";
        _existingSurveys = null;
        _selectedSurvey = new DTOSurvey();
        _surveys = new List<DTOSurvey>();
        try
        {
            List<Shared.Survey> surveys = await Service.GetAllSurveysAsync();

            foreach (Shared.Survey survey in surveys)
            {
                _surveys.Add(Service.ConvertSurveyToDTO(survey));
            }

            if (_surveys.Count > 0)
            {
                _selectedSurvey = _surveys.First();
            }

            _existingSurveys = _selectedSurvey != null;
        }
        catch (Exception ex)
        {
            strError = ex.GetBaseException().Message;
        }
    }

    private async void OnSurveySubmit(object? sender, EventArgs args)
    {
        await LoadSurveyResultsData();
    }

    private async Task RefreshSurveys(Guid surveyId)
    {
        Validate();
        List<Shared.Survey> surveys = await Service.GetAllSurveysAsync();

        foreach (Shared.Survey survey in surveys)
        {
            _surveys.Add(Service.ConvertSurveyToDTO(survey));
        }

        _selectedSurvey = _surveys.Where(x => x.Id == surveyId).FirstOrDefault();
    }

    private async Task SelectedSurveyChange(object value)
    {
        if (_selectedSurvey is null)
        {
            return;
        }

        _surveys = new List<DTOSurvey>();
        await RefreshSurveys(_selectedSurvey.Id);

        await LoadSurveyResultsData();
    }

    [MemberNotNull(nameof(_surveys))]
    private void Validate()
    {
        if (_surveys == null)
        {
            throw new InvalidOperationException("Disallowed null");
        }
    }
}
