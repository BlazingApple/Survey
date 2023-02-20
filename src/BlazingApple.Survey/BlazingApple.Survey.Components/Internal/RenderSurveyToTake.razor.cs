using BlazingApple.Survey.Components.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Diagnostics.CodeAnalysis;

namespace BlazingApple.Survey.Components.Internal;

/// <summary>Allows a consumer to take a survey.</summary>
public partial class RenderSurveyToTake : ComponentBase
{
    private bool SeeResults = false;

    private bool ShowSurveyComplete = false;

    private string strError = "";

    [Inject]
    private ISurveyClient Service { get; set; } = null!;

    /// <summary>Whether or not to allow consumers/users to see the results after they complete the survey.</summary>
    [Parameter]
    public bool AllowSeeResults { get; set; } = true;

    /// <summary>The text to show if there are no questions in the survey.</summary>
    [Parameter]
    public string NoQuestionsText { get; set; } = "There aren't any questions to answer!";

    /// <summary>Called when the survey is submitted by the user.</summary>
    [Parameter]
    public EventHandler? OnSubmit { get; set; }

    /// <summary>
    /// Pass this if you'd like to override the default route to post the survey to.
    /// </summary>
    [Parameter]
    public string? Route { get; set; }

    /// <summary>The survey to allow the user to take.</summary>
    [Parameter, EditorRequired]
    public DTOSurvey? SelectedSurvey { get; set; }

    /// <summary>What to show when the survey has been completed.</summary>
    [Parameter]
    public string? SurveyCompleteText { get; set; } = "Survey Complete";

    /// <summary>
    /// User taking the survey.
    /// </summary>
    [Parameter, EditorRequired]
    public string UserId { get; set; } = null!;

    private void CompleteSurvey()
    {
        Validate();
        ShowSurveyComplete = true;

        // Clear Answers
        foreach (DTOQuestion question in SelectedSurvey.Questions!)
        {
            question.AnswerValueString = null;
            question.AnswerValueDateTime = null;
            question.AnswerValueList = null;
        }
    }

    private void OnSeeResultsClick(MouseEventArgs args)
    {
        SeeResults = true;
    }

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
            bool result = await @Service.TakeSurvey(SelectedSurvey, UserId, Route);

            CompleteSurvey();
        }
        catch (Exception ex)
        {
            strError = ex.GetBaseException().Message;
        }

        OnSubmit?.Invoke(this, EventArgs.Empty);
    }

    [MemberNotNull(nameof(SelectedSurvey))]
    private void Validate()
    {
        if (SelectedSurvey is null)
        {
            throw new ArgumentNullException(nameof(SelectedSurvey));
        }
    }
}
