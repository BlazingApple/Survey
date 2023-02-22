namespace BlazingApple.Survey.Shared;

/// <summary>CRUD Request for changing a <see cref="Survey" />.</summary>
public record SurveyRequest(UserAction Action, Survey Record) : ISurveyRequest<Survey>
{
}

/// <summary>CRUD Request for changing a <see cref="Question" />.</summary>
public record ItemRequest(UserAction Action, Question Record, Survey Survey) : ISurveyRequest<Question>
{
}
