using BlazingApple.Survey.Components.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;

namespace BlazingApple.Survey.Components.Internal;

/// <summary>
/// Renders the admin view of a single survey.
/// </summary>
public partial class RenderSurveyCardAdmin : ComponentBase
{
	private bool _isEditing;

	/// <summary>
	/// Additional route segments when posting or editing a survey
	/// </summary>
	[Parameter]
	public string? AdditionalSegments { get; set; }

	/// <summary>
	/// Called after the survey is deleted.
	/// </summary>
	[Parameter]
	public EventCallback<Shared.Survey> OnDelete { get; set; }

	/// <summary>
	/// Called after the survey is updated.
	/// </summary>
	[Parameter]
	public EventCallback<Shared.Survey> OnUpdate { get; set; }

	/// <summary>
	/// The survey to render.
	/// </summary>
	[Parameter, EditorRequired]
	public Shared.Survey? Survey { get; set; }

	/// <summary>
	/// The maximum number of <see cref="QuestionOption"/> to render below a question.
	/// </summary>
	[Parameter]
	public int MaxOptionsDisplay { get; set; } = 2;

	/// <summary>
	/// Content to render in the header of each survey card.
	/// </summary>
	[Parameter]
	public RenderFragment<Shared.Survey>? SurveyHeaderContent { get; set; }

	/// <summary>
	/// Content to render in the body of the survey cards.
	/// </summary>
	[Parameter]
	public RenderFragment<Shared.Survey>? SurveyBodyContent { get; set; }

	[Inject]
	private ISurveyClient Service { get; set; } = null!;

	[Inject]
	private DialogService DialogService { get; set; } = null!;

	private void OnEditSurveyClick(MouseEventArgs args)
	{
		_isEditing = !_isEditing;
	}

	private async Task OnUpdateInternal()
	{
		if (OnUpdate.HasDelegate)
		{
			await OnUpdate.InvokeAsync();
		}
	}

	private async Task Delete(Shared.Survey survey)
	{
		bool result = await @Service.DeleteSurveyAsync(survey);

		if (!result)
		{
			throw new InvalidDataException("Error deleting survey");
		}
		else if (OnDelete.HasDelegate)
		{
			await OnDelete.InvokeAsync(Survey);
		}

		SurveyRequest response = new(UserAction.Delete, survey);
		DialogService.Close(response);
	}
}
