using BlazingApple.Survey.Shared;
using BlazingAppleConsumer.Server.Models;
using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Shared = BlazingApple.Survey.Shared;

namespace BlazingAppleConsumer.Server.Data
{
	public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
	{
		public DbSet<Answer> Answers { get; set; } = null!;

		public DbSet<QuestionOption> QuestionOptions { get; set; } = null!;

		public DbSet<Question> Questions { get; set; } = null!;

		public DbSet<Shared.Survey> Surveys { get; set; } = null!;

		/// <summary>DI Constructor</summary>
		public ApplicationDbContext(DbContextOptions options, IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
		{
		}
	}
}
