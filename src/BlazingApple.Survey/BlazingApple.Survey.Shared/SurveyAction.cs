using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazingApple.Survey.Shared;

/// <summary>CRUD Request for changing a <see cref="Survey" />.</summary>
public record SurveyRequest(UserAction Action, Survey Record) : ISurveyRequest<Survey>
{
}

/// <summary>CRUD Request for changing a <see cref="SurveyItem" />.</summary>
public record ItemRequest(UserAction Action, SurveyItem Record) : ISurveyRequest<SurveyItem>
{
}
