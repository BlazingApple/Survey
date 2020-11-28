using BlazingAppleConsumer.Server.Models;
using BlazingApple.Survey.Shared;
using apple=BlazingApple.Survey.Shared;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazingAppleConsumer.Server.Data
{
	public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
	{
		public ApplicationDbContext(
			DbContextOptions options,
			IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
		{
		}

		public DbSet<apple.Survey> Surveys { get; set; }
		public DbSet<SurveyAnswer> SurveyAnswers { get; set; }
		public DbSet<SurveyItem> SurveyItems { get; set; }
		public DbSet<SurveyItemOption> SurveyItemOptions { get; set; }
	}
}
