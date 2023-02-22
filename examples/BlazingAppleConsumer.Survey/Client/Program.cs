using BlazingApple.Survey.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Radzen;
using StardustDL.RazorComponents.Markdown;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlazingAppleConsumer.Survey.Client
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);
			builder.RootComponents.Add<App>("#app");

			builder.Services.AddHttpClient("BlazingAppleConsumer.Survey.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
				.AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

			// Supply HttpClient instances that include access tokens when making requests to the server project
			builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("BlazingAppleConsumer.Survey.ServerAPI"));

			builder.Services.AddApiAuthorization();
			builder.Services.AddScoped<DialogService>();
			builder.Services.AddScoped<TooltipService>();
			builder.Services.AddScoped<NotificationService>();
			builder.Services.AddSurveyClients();
			builder.Services.AddMarkdownComponent();

			await builder.Build().RunAsync();
		}
	}
}
