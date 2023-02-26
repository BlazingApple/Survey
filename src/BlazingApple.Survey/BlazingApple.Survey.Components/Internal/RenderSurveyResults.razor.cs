using BlazingApple.Survey.Components.Services;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace BlazingApple.Survey.Components.Internal;


/// <summary>
/// Render the results for a <see cref="Survey"/>
/// </summary>
public partial class RenderSurveyResults : ComponentBase
{
    [Inject]
    private ISurveyClient Service { get; set; } = null!;

    /// <inheritdoc cref="DTOSurvey"/>
    [Parameter, EditorRequired]
    public DTOSurvey? SelectedSurvey { get; set; }

    /// <summary>
    /// The route override to use when requesting the results.
    /// </summary>
    [Parameter]
    public string? ResultsRoute { get; set; }

    /// <summary>
    /// The route override to use when requesting the survey.
    /// </summary>
    [Parameter]
    public string? Route { get; set; }

    // Survey Results
    private int _numberOfQuestions;

    /// <summary>
    /// Set of questions in survey.
    /// </summary>
    private IEnumerable<DTOQuestion>? _surveyResultSet;

    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        LoadDataArgs args = new();
        await LoadSurveyResultsData(args);
    }

    private async Task LoadSurveyResultsData(LoadDataArgs loadArgs)
    {
        if (SelectedSurvey is null)
        {
            return;
        }
        LoadArgs args = new(loadArgs.Skip ?? default, loadArgs.Top ?? 1);

        _numberOfQuestions = SelectedSurvey.Questions?.Count ?? (await Service.GetSurvey(SelectedSurvey.Id, Route)).Questions?.Count ?? 0;
        _surveyResultSet = await Service.GetSurveyResults(SelectedSurvey.Id, args, ResultsRoute);
        await InvokeAsync(StateHasChanged);
    }
}
