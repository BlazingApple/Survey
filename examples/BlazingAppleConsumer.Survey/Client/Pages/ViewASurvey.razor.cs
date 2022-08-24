using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System;
using BlazingApple.Survey.Shared;

namespace BlazingAppleConsumer.Survey.Client.Pages;

public partial class ViewASurvey : ComponentBase
{
    private List<BlazingApple.Survey.Shared.Survey> _surveys;
    private Guid surveyId = Guid.Empty;

    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        _surveys = await http.GetFromJsonAsync<List<BlazingApple.Survey.Shared.Survey>>("api/surveys");
        int count = _surveys.Count;
        if (count > 0)
        {
            int randomIndex = new Random().Next(0, count);
            surveyId = _surveys[randomIndex].Id;
        }
    }
}
