using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazingApple.Survey.Shared;

namespace BlazingApple.Survey.Components;

/// <summary>Main component to render a survey for a participant to fill out.</summary>
public partial class SurveyTaker : OwningComponentBase<SurveyService>
{
    private Shared.Survey? _survey;

    private DTOSurvey? _surveyDTO;

    /// <summary>The identifier for the survey to render for the user to take.</summary>
    [Parameter, EditorRequired]
    public Guid SurveyId { get; set; }

    /// <inheritdoc />
    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();

        _survey = await Service.GetSurvey(SurveyId);
        _surveyDTO = Service.ConvertSurveyToDTO(_survey);
    }
}
