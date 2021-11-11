// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CleanArchitecture.Razor.Infrastructure.Identity;
using CleanArchitecture.Razor.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Filters;
namespace SmartAdmin.WebUI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Files");
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            filePath = Path.Combine(filePath, "Documents");
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var context = services.GetRequiredService<ApplicationDbContext>();

                    if (context.Database.IsSqlServer() || context.Database.IsSqlite())
                    {
                        context.Database.Migrate();
                    }

                    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                    var roleManager = services.GetRequiredService<RoleManager<ApplicationRole>>();

                    await ApplicationDbContextSeed.SeedDefaultUserAsync(userManager, roleManager);
                    await ApplicationDbContextSeed.SeedSampleDataAsync(context);
                    //  await ApplicationDbContextSeed.SeedSampleProductDataAsync(context);
                    await ApplicationDbContextSeed.SeekDirectionAndCategory(context);
                }
                catch (Exception ex)
                {
                    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

                    logger.LogError(ex, "An error occurred while migrating or seeding the database.");

                    throw;
                }
            }

            await host.RunAsync();
        }
        private static string[] filters = new string[] { "Microsoft.EntityFrameworkCore.Model.Validation", "WorkflowCore.Services.WorkflowHost", "WorkflowCore.Services.BackgroundTasks.RunnablePoller", "Microsoft.Hosting.Lifetime", "Serilog.AspNetCore.RequestLoggingMiddleware" };
        public static IHostBuilder CreateHostBuilder(string[] args) =>

            Host.CreateDefaultBuilder(args)
                .UseSerilog((context, services, configuration) => configuration
                    .ReadFrom.Configuration(context.Configuration)
                    .ReadFrom.Services(services)
                    .Enrich.FromLogContext()
                    .Enrich.WithClientIp()
                    .Enrich.WithClientAgent()

                    .Filter.ByExcluding(
                        /* (logevent) => {
                             Console.WriteLine(logevent);
                             return false;
                             }*/
                        Matching.WithProperty<string>("SourceContext", p => filters.Contains(p))
                        )
                    .WriteTo.Console()
                    , writeToProviders: true)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
