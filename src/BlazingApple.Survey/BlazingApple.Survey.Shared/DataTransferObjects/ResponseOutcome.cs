namespace BlazingApple.Survey.Shared.DataTransferObjects;

/// <summary>
/// Response for a request.
/// </summary>
public enum ResponseOutcome
{
	/// <summary>
	/// A poorly formatted request, error on consuming side.
	/// </summary>
	BadRequest,
	/// <summary>
	/// Attempted to add where record already exists.
	/// </summary>
	Conflict,
	/// <summary>
	/// Requested resource not found.
	/// </summary>
	NotFound,
	/// <summary>
	/// Unknown Error
	/// </summary>
	Error,
	/// <summary>
	/// Success
	/// </summary>
	Success,
}
