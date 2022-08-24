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
        public DbSet<Answer> Answers { get; set; } = null!;

        public DbSet<QuestionOption> QuestionOptions { get; set; } = null!;

        public DbSet<Question> Questions { get; set; } = null!;

        public DbSet<apple.Survey> Surveys { get; set; } = null!;

        /// <summary>DI Constructor</summary>
        public ApplicationDbContext(DbContextOptions options, IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }
    }
}
