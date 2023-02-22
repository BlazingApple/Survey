using Microsoft.AspNetCore.Components;

namespace BlazingApple.Survey.Components.Internal.Questions;

/// <summary>Renders a particular <see cref="Shared.Question" /></summary>
public partial class RenderQuestion : ComponentBase
{
	private string _idString = null!;

	/// <summary>The <see cref="Shared.Question" /> to render.</summary>
	[Parameter, EditorRequired]
	public DTOQuestion? QuestionAnswer { get; set; }

	/// <summary>
	/// Whether to disable the component.
	/// </summary>
	[Parameter]
	public bool IsDisabled { get; set; }

	/// <inheritdoc />
	protected override void OnInitialized()
	{
		base.OnInitialized();
		_idString = "itemControl-" + QuestionAnswer?.Position;

	}
}
