using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazingApple.Survey.Components.Internal;

/// <summary>Allows a consumer to take a survey.</summary>
public class RenderSurveyToTake : OwningComponentBase<SurveyService>
{
    private bool SeeResults = false;

    private bool ShowSurveyComplete = false;

    private string strError = "";

    /// <summary>Whether or not to allow consumers/users to see the results after they complete the survey.</summary>
    [Parameter]
    public bool AllowSeeResults { get; set; } = true;

    /// <summary>The text to show if there are no questions in the survey.</summary>
    [Parameter]
    public string NoQuestionsText { get; set; } = "There aren't any questions to answer!";

    /// <summary>Called when the survey is submitted by the user.</summary>
    [Parameter]
    public EventHandler? OnSubmit { get; set; }

    /// <summary>The survey to allow the user to take.</summary>
    [Parameter, EditorRequired]
    public DTOSurvey? SelectedSurvey { get; set; }

    /// <summary>What to show when the survey has been completed.</summary>
    [Parameter]
    public string? SurveyCompleteText { get; set; } = "Survey Complete";

    private void CompleteSurvey()
    {
        Validate();
        ShowSurveyComplete = true;

        // Clear Answers
        foreach (DTOSurveyItem item in SelectedSurvey.SurveyItems)
        {
            item.AnswerValueString = null;
            item.AnswerValueDateTime = null;
            item.AnswerValueList = null;
        }
    }

    private void OnSeeResultsClick(MouseEventArgs args) => SeeResults = true;

    private void OnStartOverClick(MouseEventArgs args)
    {
        SeeResults = false;
        ShowSurveyComplete = false;
    }

    /// <summary>Save and submit the survey to the database</summary>
    /// <returns></returns>
    private async Task SaveSurvey()
    {
        Validate();

        try
        {
            var result = await @Service.CreateSurveyAnswersAsync(SelectedSurvey);

            CompleteSurvey();
        }
        catch (Exception ex)
        {
            strError = ex.GetBaseException().Message;
        }

        if (OnSubmit != null)
            OnSubmit.Invoke(this, null);
    }

    [MemberNotNull(nameof(SelectedSurvey))]
    private void Validate()
    {
        if (SelectedSurvey is null)
            throw new ArgumentNullException(nameof(SelectedSurvey));
    }
}
