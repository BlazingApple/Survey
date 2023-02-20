using Microsoft.Extensions.DependencyInjection;

namespace BlazingApple.Survey.Shared.Services;

/// <summary>Supports registration of <see cref="SurveyService" /></summary>
public static class ServiceCollectionExtensions
{
	/// <summary>
	/// Add survey services.
	/// </summary>
	/// <param name="services"><see cref="ServiceCollection"/></param>
	/// <returns><see cref="ServiceCollection"/> for fluent API.</returns>
	public static IServiceCollection AddSurveys(this IServiceCollection services)
	{
		services.AddScoped<ISurveyService, SurveyService>();
		return services; ;
	}
}
