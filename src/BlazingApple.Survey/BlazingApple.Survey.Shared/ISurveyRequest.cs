using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazingApple.Survey.Shared;

/// <summary>Represents a CRUD operation for a <see cref="BlazingApple.Survey" /> entity.</summary>
/// <typeparam name="TEntity"></typeparam>
public interface ISurveyRequest<TEntity>
    where TEntity : class
{
    /// <inheritdoc cref="UserAction" />
    public UserAction Action { get; }

    /// <summary>The type of entity changing.</summary>
    public TEntity Record { get; }
}
