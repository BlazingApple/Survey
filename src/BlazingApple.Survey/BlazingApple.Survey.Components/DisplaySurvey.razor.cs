using BlazingApple.Survey.Components.Internal;
using Microsoft.AspNetCore.Components;
using Radzen;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazingApple.Survey.Components;

/// <summary>Displays a survey for an end user.</summary>
public partial class DisplaySurvey : OwningComponentBase<SurveyService>
{
    private bool? _existingSurveys;
    private DTOSurvey? _selectedSurvey;
    private List<DTOSurvey>? _surveys;
    private string? strError;

    private IEnumerable<DTOSurveyItem> SurveyResults;

    // Survey Results
    private int SurveyResultsCount;

    private RenderSurveyResults surveyResultsRef;

    [Inject]
    private DialogService DialogService { get; set; } = null!;

    /// <summary>Load survey results.</summary>
    /// <param name="args"></param>
    /// <returns>Async op.</returns>
    public async Task LoadSurveyResultsData(LoadDataArgs args)
    {
        SurveyResultsCount = await Service.GetSurveyResultsCount(_selectedSurvey.Id);
        SurveyResults = await Service.GetSurveyResults(_selectedSurvey.Id, args);
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

            foreach (var survey in surveys)
                _surveys.Add(Service.ConvertSurveyToDTO(survey));

            if (_surveys.Count > 0)
                _selectedSurvey = _surveys.First();

            _existingSurveys = _selectedSurvey != null;
        }
        catch (Exception ex)
        {
            strError = ex.GetBaseException().Message;
        }
    }

    // SelectedSurveyChange

    private async void OnSurveySubmit(object sender, EventArgs args)
    {
        LoadDataArgs loadArgs = new LoadDataArgs();
        loadArgs.Filter = null;
        loadArgs.OrderBy = null;
        loadArgs.Skip = 0;
        loadArgs.Top = 1;
        await LoadSurveyResultsData(loadArgs);
    }

    private async Task RefreshSurveys(Guid surveyId)
    {
        Validate();
        List<Shared.Survey> surveys = await Service.GetAllSurveysAsync();

        foreach (var survey in surveys)
            _surveys.Add(Service.ConvertSurveyToDTO(survey));

        _selectedSurvey = _surveys.Where(x => x.Id == surveyId).FirstOrDefault();
    }

    private async Task SelectedSurveyChange(object value)
    {
        _surveys = new List<DTOSurvey>();
        await RefreshSurveys(_selectedSurvey.Id);

        // Refresh results
        LoadDataArgs args = new LoadDataArgs();
        args.Filter = null;
        args.OrderBy = null;
        args.Skip = 0;
        args.Top = 1;

        await LoadSurveyResultsData(args);
    }

    [MemberNotNull(nameof(_surveys))]
    private void Validate()
    {
        if (_surveys == null)
            throw new InvalidOperationException("Disallowed null");
    }
}
