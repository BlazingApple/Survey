using BlazingApple.Survey.Components.Services;
using Microsoft.AspNetCore.Components;

namespace BlazingApple.Survey.Components;

/// <summary>Main component to render a survey for a participant to fill out.</summary>
public partial class SurveyTaker : ComponentBase
{
	private Shared.Survey? _survey;

	private DTOSurvey? _surveyDTO;

	[Inject]
	private ISurveyClient Service { get; set; } = null!;

	/// <summary>The identifier for the survey to render for the user to take.</summary>
	[Parameter]
	public Guid SurveyId { get; set; }

	/// <summary>
	/// The survey. Required if <see cref="SurveyId"/> is not provided.
	/// </summary>
	[Parameter]
	public Shared.Survey? Survey { get; set; }

	/// <summary>
	/// Pass this if you'd like to override the default route to post the survey to.
	/// </summary>
	[Parameter]
	public string? AnswersRoute { get; set; }

	/// <summary>
	/// Pass this if you'd like to override the default route to request the survey from.
	/// </summary>
	[Parameter]
	public string? SurveyRoute { get; set; }

	/// <summary>
	/// User taking the survey.
	/// </summary>
	[Parameter, EditorRequired]
	public string UserId { get; set; } = null!;

	/// <summary>
	/// The route override string used when requesting the routes.
	/// </summary>
	[Parameter]
	public string? ResultsRoute { get; set; }

	/// <inheritdoc />
	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();

		if (SurveyId != Guid.Empty)
		{
			_survey = await Service.GetSurvey(SurveyId, SurveyRoute);
		}
		else if (Survey is not null)
		{
			_survey = Survey;
			_surveyDTO = Service.ConvertSurveyToDTO(_survey);
		}
		else
		{
			throw new ArgumentNullException(nameof(Survey));
		}
	}
}
