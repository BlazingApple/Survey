using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazingApple.Survey.Shared;

/// <summary>Type of CRUD action the user took.</summary>
public enum UserAction
{
    /// <summary>Create a new <see cref="Shared.Survey" /> or other entity.</summary>
    Create,

    /// <summary>Update an existing <see cref="Shared.Survey" /> or other entity.</summary>
    Update,

    /// <summary>Delete an existing <see cref="Shared.Survey" /> or other entity.</summary>
    Delete
}
