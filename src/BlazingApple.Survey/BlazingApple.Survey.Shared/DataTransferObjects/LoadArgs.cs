using System.ComponentModel;

namespace BlazingApple.Survey.Shared.DataTransferObjects;

/// <summary>
/// Load arguments
/// </summary>
public class LoadArgs
{
	/// <summary>
	/// Records to skip
	/// </summary>
	public int Skip { get; set; }

	/// <summary>
	/// Top most records to take
	/// </summary>
	public int Top { get; set; }

	/// <summary>
	/// The sort direction.
	/// </summary>
	public ListSortDirection? SortDirection { get; set; }

	/// <summary>
	/// Default Constructor
	/// </summary>
	public LoadArgs() { }

	/// <summary>
	/// Quick constructor.
	/// </summary>
	public LoadArgs(int skip, int top, ListSortDirection? sortDirection = null)
	{
		Skip = skip;
		Top = top;
		SortDirection = sortDirection;
	}
}
