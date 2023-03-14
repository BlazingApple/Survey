using Microsoft.AspNetCore.Components;

namespace BlazingApple.Survey.Components.Internal;

/// <summary>Render the result for a single question.</summary>
public partial class RenderSurveyResult : ComponentBase
{
	/// <summary>The name of the question or <see cref="Question" /></summary>
	[Parameter, EditorRequired]
	public string Label { get; set; } = null!;

	/// <summary>The responses received.</summary>
	[Parameter, EditorRequired]
	public List<AnswerResponse>? Responses { get; set; }

	private List<AnswerResponseDisplay>? ResponseDisplays => Responses?.ConvertAll(ar => new AnswerResponseDisplay(Truncate(ar.OptionLabel, 256), ar.Responses));

	private static string Truncate(string value, int maxLength)
		=> string.IsNullOrWhiteSpace(value) ? value : value.Length <= maxLength ? value : $"{value[..maxLength]}...";

	private record AnswerResponseDisplay(
		string OptionLabel,
		double Responses)
	{ };
}
