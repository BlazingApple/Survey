using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazingApple.Survey.Components;

/// <summary>Render results for a given survey.</summary>
public partial class SurveyResults : OwningComponentBase<SurveyService>
{
    private Shared.Survey? _survey;
    private DTOSurvey? _surveyDto;

    /// <summary>The identifier of the survey for which to retrieve results.</summary>
    [Parameter]
    public Guid SurveyId { get; set; }

    /// <inheritdoc />
    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();

        _survey = await Service.GetSurvey(SurveyId);
        _surveyDto = Service.ConvertSurveyToDTO(_survey);
    }
}
