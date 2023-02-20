using BlazingApple.Survey.Components.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BlazingApple.Survey.Components;

/// <summary>
/// Extensions to add <see cref="Survey"/> client services.
/// </summary>
public static class ServiceCollectionExtensions
{
	/// <summary>
	/// Add <see cref="Survey"/> Http client services.
	/// </summary>
	/// <param name="services"><see cref="IServiceCollection"/></param>
	/// <returns>Fluent API.</returns>
	public static IServiceCollection AddSurveyClients(this IServiceCollection services)
	{
		services.AddScoped<ISurveyClient, SurveyClient>();
		return services;
	}
}
