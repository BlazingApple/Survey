using BlazingAppleConsumer.Server.Models;
using BlazingApple.Survey.Shared;

using apple = BlazingApple.Survey.Shared;

using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Duende.IdentityServer.EntityFramework.Options;

namespace BlazingAppleConsumer.Server.Data
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        public DbSet<SurveyAnswer> SurveyAnswers { get; set; }

        public DbSet<SurveyItemOption> SurveyItemOptions { get; set; }

        public DbSet<SurveyItem> SurveyItems { get; set; }

        public DbSet<apple.Survey> Surveys { get; set; }

        public ApplicationDbContext(
                                            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }
    }
}
