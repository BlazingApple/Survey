using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using Radzen;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using StardustDL.RazorComponents.Markdown;

namespace BlazingAppleConsumer.Survey.Client
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebAssemblyHostBuilder.CreateDefault(args);
			builder.RootComponents.Add<App>("#app");

			builder.Services.AddHttpClient("BlazingAppleConsumer.Survey.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
				.AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

			// Supply HttpClient instances that include access tokens when making requests to the server project
			builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("BlazingAppleConsumer.Survey.ServerAPI"));

			builder.Services.AddApiAuthorization();
			builder.Services.AddScoped<DialogService>();
			builder.Services.AddScoped<TooltipService>();
			builder.Services.AddScoped<NotificationService>();
			builder.Services.AddScoped<BlazingApple.SurveyService>();
			builder.Services.AddMarkdownComponent();

			await builder.Build().RunAsync();
		}
	}
}
