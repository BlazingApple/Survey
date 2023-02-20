namespace BlazingApple.Survey.Shared.DataTransferObjects;

/// <summary>The data transfer object for <see cref="Survey" /></summary>
/// <seealso cref="Survey" />
public partial class DTOSurvey
{
	/// <inheritdoc cref="Survey.DateCreated" />
	public DateTime DateCreated { get; set; }

	/// <inheritdoc cref="Survey.DateUpdated" />
	public DateTime? DateUpdated { get; set; }

	/// <summary>The Key</summary>
	public Guid Id { get; set; }

	/// <inheritdoc cref="Survey.Name" />
	public string? Name { get; set; }

	/// <inheritdoc cref="DTOQuestion" />
	public List<DTOQuestion>? Questions { get; set; }

	/// <inheritdoc cref="Survey.UserId" />
	public string? UserId { get; set; }
}
