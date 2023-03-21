using BlazingApple.Survey.Components.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using System.Diagnostics.CodeAnalysis;

namespace BlazingApple.Survey.Components;

/// <summary>Admin operations for conducting CRUD operations for surveys and their questions.</summary>
public partial class SurveyAdmin : ComponentBase
{
	private Shared.Survey? _newSurvey;
	private bool _isCreatingNewSurvey;
	private List<Shared.Survey>? _surveys;
	private readonly string strError = "";

	[Inject]
	private ISurveyClient Service { get; set; } = null!;

	/// <summary>
	/// Additional route segments when posting or editing a survey
	/// </summary>
	[Parameter]
	public string? AdditionalSegments { get; set; }

	/// <summary>
	/// Content to render in the header of each survey card.
	/// </summary>
	[Parameter]
	public RenderFragment<Shared.Survey>? SurveyHeaderContent { get; set; }

	/// <summary>
	/// Content to render at the end of the body of the survey cards.
	/// </summary>
	[Parameter]
	public RenderFragment<Shared.Survey>? SurveyFooterContent { get; set; }

	/// <summary>
	/// The maximum number of <see cref="QuestionOption"/> to render below a question.
	/// </summary>
	[Parameter]
	public int MaxOptionsDisplay { get; set; } = 2;

	/// <summary>
	/// Content to render at the start of the body of the survey cards.
	/// </summary>
	[Parameter]
	public RenderFragment<Shared.Survey>? SurveyBodyContent { get; set; }

	/// <summary>
	/// Shown when loading
	/// </summary>
	[Parameter]
	public RenderFragment? LoadingContent { get; set; }

	/// <summary>
	/// Survey for the new survey button.
	/// </summary>
	[Parameter]
	public string? NewSurveyLabel { get; set; } = "Create new survey";

	[Inject]
	private DialogService DialogService { get; set; } = null!;

	private bool? ExistingSurveys => _surveys?.Count > 0;

	/// <summary>
	/// Provide to this to override the route at which the surveys are requested.
	/// </summary>
	[Parameter]
	public string? SurveyRetrievalRoute { get; set; }

	/// <inheritdoc />
	protected override async Task OnInitializedAsync()
	{
		_surveys = new List<Shared.Survey>();
		_newSurvey = new Shared.Survey();

		DialogService.OnClose += DialogClose; // detect when a dialog has closed

		_surveys = await @Service.GetAllSurveysAsync(SurveyRetrievalRoute);
	}

	private static void RemoveAndAdd<T>(ICollection<T> collection, T toRemove, T toAdd)
	{
		collection.Remove(toRemove);
		collection.Add(toAdd);
	}

	private async Task Delete(Shared.Survey survey)
	{
		Validate();
		_surveys.Remove(survey);
		await RefreshSurveys();
	}

	private async void DialogClose(dynamic result)
	{
		Validate();
		if (_newSurvey is null)
		{
			throw new InvalidOperationException("Disallowed null for the selected survey");
		}

		if (result != null)
		{
			if (result is ItemRequest itemRequest)
			{
				await ProcessItemRequest(itemRequest);
			}
			else if (result is SurveyRequest surveyRequest)
			{
				ProcessSurveyRequest(surveyRequest);
			}
		}

		StateHasChanged();
	}

	private void OnNewSurveyClick(MouseEventArgs args)
	{
		_isCreatingNewSurvey = !_isCreatingNewSurvey;
	}

	private async Task ProcessItemRequest(ItemRequest itemRequest)
	{
		await RefreshSurveys();
	}

	private void ProcessSurveyRequest(SurveyRequest surveyRequest)
	{
		UserAction operation = surveyRequest.Action;
		Validate();

		Shared.Survey? surveyToEdit = _surveys.Where(x => x.Id == surveyRequest.Record.Id).FirstOrDefault();
		if (operation == UserAction.Create)
		{
			// Survey does not exist - Add it
			if (surveyToEdit is null)
			{
				_surveys.Add(surveyRequest.Record);
			}
		}
		else if (operation == UserAction.Update)
		{
			RemoveAndAdd(_surveys!, surveyToEdit, surveyRequest.Record);
		}
		else if (operation == UserAction.Delete)
		{
			Shared.Survey? surveyToDelete = _surveys.Where(x => x.Id == surveyRequest.Record.Id).FirstOrDefault();

			if (surveyToDelete != null)
			{
				_surveys.Remove(surveyToDelete);
			}
		}
	}

	private async Task RefreshSurveys()
	{
		_surveys = await Service.GetAllSurveysAsync(SurveyRetrievalRoute);
	}

	[MemberNotNull(nameof(_surveys))]
	private void Validate()
	{
		if (_surveys is null)
		{
			throw new ArgumentNullException(nameof(_surveys), "Invalid null.");
		}
	}
}
