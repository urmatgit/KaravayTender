using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using CleanArchitecture.Razor.Application.Settings;
using Hangfire;
using Hangfire.MemoryStorage;
using HandfireJobs;
using Microsoft.Extensions.Localization;
using Hangfire.SQLite;

namespace CleanArchitecture.Razor.HandfireJobs
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddHangfire(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddHangfire(x=>x.UseMemoryStorage());
            }
            else if (configuration.GetValue<bool>("UseSqliteDatabase"))
            {
                services.AddHangfire(x => x.UseSQLiteStorage(configuration.GetConnectionString("DefaultConnectionSqlite")));
                
            }
            else
            {
                services.AddHangfire(x => x.UseSqlServerStorage(configuration.GetConnectionString("DefaultConnection")));
            }
            
            return services;
        }
        public static IApplicationBuilder UseHandfire(this IApplicationBuilder app,string caption)
        {
            app.UseHangfireDashboard("/jobs", new DashboardOptions
            {
                DashboardTitle = caption,
                Authorization = new[] { new HangfireAuthorizationFilter() },

            });

            return app;
        }
    }
}
