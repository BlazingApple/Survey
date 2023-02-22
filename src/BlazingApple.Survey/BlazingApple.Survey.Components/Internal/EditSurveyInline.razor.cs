using Microsoft.AspNetCore.Components;

namespace BlazingApple.Survey.Components.Internal;

/// <summary>
/// Allows editing a survey without a modal.
/// </summary>
public partial class EditSurveyInline : ComponentBase
{
    /// <summary>
    /// <c>true</c> if is open, <c>false</c> otherwise.
    /// </summary>
    [Parameter]
    public bool Value { get; set; }

    /// <summary>
    /// The callback for the value being changed.
    /// </summary>
    [Parameter]
    public EventCallback<bool> ValueChanged { get; set; }

    /// <summary>
    /// Additional route segments when posting or editing a survey
    /// </summary>
    [Parameter]
    public string? AdditionalSegments { get; set; }

    /// <summary>
    /// The survey that's being edited.
    /// </summary>
    [Parameter, EditorRequired]
    public Shared.Survey? SelectedSurvey { get; set; }

    /// <summary>
    /// The bound data element indicating that the survey is in edit mode or not.
    /// </summary>
    public bool BoundValue
    {
        get => Value;
        set
        {
            ValueChanged.InvokeAsync(value);
            if (OnClose.HasDelegate)
                OnClose.InvokeAsync();

		}
    }

	/// <summary>
	/// Called when the survey edit is closed.
	/// </summary>
	[Parameter]
	public EventCallback OnClose { get; set; }

    private void OnSurveyEditClosed(object? sender, EventArgs args)
    {
        BoundValue = false;
    }
    private void OnButtonClick()
    {
        BoundValue = !BoundValue;
    }
}
