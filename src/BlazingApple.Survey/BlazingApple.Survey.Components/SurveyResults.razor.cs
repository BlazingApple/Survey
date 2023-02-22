using BlazingApple.Survey.Components.Services;
using Microsoft.AspNetCore.Components;

namespace BlazingApple.Survey.Components;

/// <summary>Render results for a given survey.</summary>
public partial class SurveyResults : ComponentBase
{
    private Shared.Survey? _survey;
    private DTOSurvey? _surveyDto;

    [Inject]
    private ISurveyClient Service { get; set; } = null!;

    /// <summary>
    /// Pass this if you'd like to override the default route to post the survey to.
    /// </summary>
    [Parameter]
    public string? Route { get; set; }

    /// <summary>The identifier of the survey for which to retrieve results.</summary>
    [Parameter]
    public Guid SurveyId { get; set; }

    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        _survey = await Service.GetSurvey(SurveyId, Route);
        _surveyDto = Service.ConvertSurveyToDTO(_survey);
    }
}
