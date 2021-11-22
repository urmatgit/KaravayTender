// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CleanArchitecture.Razor.Domain.Constants;
using CleanArchitecture.Razor.Infrastructure.Identity;
using CleanArchitecture.Razor.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Filters;
namespace SmartAdmin.WebUI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Serilog.Debugging.SelfLog.Enable(msg => Debug.WriteLine(msg));
            PathConstants.CurrentDirectory = Directory.GetCurrentDirectory();
            var filePath = Path.Combine(PathConstants.CurrentDirectory, PathConstants.FilesPath);
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            var  filePathDoc = Path.Combine(filePath, PathConstants.DocumentsPath);
            if (!Directory.Exists(filePathDoc))
            {
                Directory.CreateDirectory(filePathDoc);
            }
            var filePathSpec= Path.Combine(filePath, PathConstants.SpecificationsPath);
            if (!Directory.Exists(filePathSpec))
            {
                Directory.CreateDirectory(filePathSpec);
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
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                    .ReadFrom.Services(services)
                    .Enrich.FromLogContext()
                    .Enrich.WithClientIp()
                    .Enrich.WithClientAgent()
                    
                    .Filter.ByExcluding(
                        //(logevent) =>
                        //{
                        //    Console.WriteLine(logevent);
                        //    var cxt = logevent.Properties.Where(x => x.Key == "SourceContext").Select(x => x.Value.ToString()).ToArray();
                        //    if (cxt.Any(x => filters.Contains(x)))
                        //    {
                        //        return false;
                        //    }
                        //    return true;
                        //}
                        Matching.WithProperty<string>("SourceContext", p => filters.Contains(p))
                        )
                    .WriteTo.Debug()
                    
                    , writeToProviders: true)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
