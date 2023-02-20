using BlazingApple.Survey.Components.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using System.Diagnostics.CodeAnalysis;

namespace BlazingApple.Survey.Components;

/// <summary>Admin operations for conducting CRUD operations for surveys and their questions.</summary>
public partial class SurveyAdmin : ComponentBase
{
	private readonly DialogOptions _options = new() { Width = "500px", Height = "280px" };
	private Shared.Survey? _selectedSurvey;
	private bool _shouldShowEditInline;
	private bool _shouldShowNewInline;
	private List<Shared.Survey>? _surveys;
	private readonly string strError = "";

	[Inject]
	private ISurveyClient Service { get; set; } = null!;

	/// <summary>Edit the survey inline if <c>true</c>, <c>false</c> otherwise.</summary>
	[Parameter]
	public bool PromptInline { get; set; }

	/// <summary>The form title for editing the survey</summary>
	[Parameter]
	public string? Title { get; set; }

	/// <summary>
	/// Additional route segments when posting or editing a survey
	/// </summary>
	[Parameter]
	public string? AdditionalSegments { get; set; }

	[Inject]
	private DialogService DialogService { get; set; } = null!;

	private bool? ExistingSurveys => _surveys?.Count > 0;

	[Inject]
	private TooltipService TooltipService { get; set; } = null!;

	/// <inheritdoc />
	protected override async Task OnInitializedAsync()
	{
		_surveys = new List<Shared.Survey>();
		_selectedSurvey = new Shared.Survey();

		DialogService.OnClose += DialogClose; // detect when a dialog has closed

		_surveys = await @Service.GetAllSurveysAsync();

		if (_surveys.Count > 0)
		{
			_selectedSurvey = _surveys.First();
		}
	}

	/// <inheritdoc />
	protected override void OnParametersSet()
	{
		base.OnParametersSet();
		if (string.IsNullOrEmpty(Title))
		{
			Title = "Manage Surveys";
		}
	}

	private static void RemoveAndAdd<T>(ICollection<T> collection, T toRemove, T toAdd)
	{
		collection.Remove(toRemove);
		collection.Add(toAdd);
	}

	private void DialogClose(dynamic result)
	{
		Validate();
		if (_selectedSurvey is null)
		{
			throw new InvalidOperationException("Disallowed null for the selected survey");
		}

		if (result != null)
		{
			if (result is ItemRequest itemRequest)
			{
				ProcessItemRequest(itemRequest);
			}
			else if (result is SurveyRequest surveyRequest)
			{
				ProcessSurveyRequest(surveyRequest);
			}
		}

		StateHasChanged();
	}

	private void OnEditSurveyClick(MouseEventArgs args)
	{
		if (PromptInline)
		{
			_shouldShowEditInline = !_shouldShowEditInline;
			if (_shouldShowEditInline)
			{
				_shouldShowNewInline = false;
			}
		}
		else if (_selectedSurvey is not null)
		{
			Dictionary<string, object> parameters = new() {
					{ nameof(EditSurvey.SelectedSurvey), _selectedSurvey },
				};

			if (!string.IsNullOrEmpty(AdditionalSegments))
			{
				parameters.Add(nameof(EditSurvey.AdditionalSegments), AdditionalSegments);
			}

			DialogService.Open<EditSurvey>($"New Survey", parameters, _options);
		}
	}

	private void OnNewSurveyClick(MouseEventArgs args)
	{
		if (PromptInline)
		{
			_shouldShowNewInline = !_shouldShowNewInline;

			if (_shouldShowNewInline)
			{
				_shouldShowEditInline = false;
			}
		}
		else
		{
			Shared.Survey newSurvey = new() { Id = Guid.Empty };
			Dictionary<string, object> parameters = new()
			{
				{ nameof(EditSurvey.SelectedSurvey), newSurvey },
			};

			if (!string.IsNullOrEmpty(AdditionalSegments))
			{
				parameters.Add(nameof(EditSurvey.AdditionalSegments), AdditionalSegments);
			}

			DialogService.Open<EditSurvey>($"New Survey", parameters, _options);
		}
	}

	private void ProcessItemRequest(ItemRequest itemRequest)
	{
		UserAction operation = itemRequest.Action;
		Question modifiedQuestion = itemRequest.Record;

		if (_selectedSurvey is null)
		{
			throw new InvalidDataException("Unexpected null survey");
		}

		// Refresh the question.
		Question? existingQuestion = _selectedSurvey.Questions.Where(x => x.Id == modifiedQuestion.Id).FirstOrDefault();

		if (existingQuestion is null)
		{
			throw new InvalidOperationException("Could not find matching survey item");
		}

		if (operation == UserAction.Delete)
		{
			_selectedSurvey.Questions.Remove(existingQuestion);
		}
		else
		{
			RemoveAndAdd(_selectedSurvey.Questions, existingQuestion, modifiedQuestion);
		}
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
				_selectedSurvey = surveyRequest.Record;
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

			_selectedSurvey = _surveys.FirstOrDefault();
		}
	}

	private async Task RefreshSurveys(Guid surveyToRefresh)
	{
		_surveys = await Service.GetAllSurveysAsync();
		_selectedSurvey = _surveys.Where(x => x.Id == surveyToRefresh).FirstOrDefault();
	}

	private async Task SelectedSurveyChange(object value)
	{
		if (_selectedSurvey == null)
		{
			return;
		}

		await RefreshSurveys(_selectedSurvey.Id);
		StateHasChanged();
	}

	private void ShowTooltip(ElementReference elementReference, TooltipOptions? options = null)
	{
		TooltipService.Open(elementReference, options?.Text, options);
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
