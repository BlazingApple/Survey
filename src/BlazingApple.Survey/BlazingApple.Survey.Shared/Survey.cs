﻿using System.ComponentModel.DataAnnotations;

namespace BlazingApple.Survey.Shared;

/// <summary>Represents a survey to be filled out by users.</summary>
public partial class Survey
{
	/// <summary>The creation date of this survey.</summary>
	public DateTime DateCreated { get; set; }

	/// <summary>The date the survey or questions were last modified.</summary>
	public DateTime? DateUpdated { get; set; }

	/// <summary>The survey's identifier.</summary>
	public Guid Id { get; set; }

	/// <summary>The display name.</summary>
	[Required(AllowEmptyStrings = false)]
	public string Name { get; set; } = null!;

	/// <summary>Whether to show the results to users after the survey was completed.</summary>
	public bool ShowResultsOnCompletion { get; set; }

	/// <summary>The list of survey questions.</summary>
	public virtual ICollection<Question> Questions { get; set; }

	/// <summary>Foreign key for the user.</summary>
	public string? UserId { get; set; }

	/// <summary>Default constructor.</summary>
	public Survey()
	{
		Questions = new HashSet<Question>();
	}
}