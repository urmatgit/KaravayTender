// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using HandfireJobs;
using Hangfire;
using Hangfire.MemoryStorage;
using Hangfire.SQLite;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.Razor.HandfireJobs
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddHangfire(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddHangfire(x => x.UseMemoryStorage());
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
        public static IApplicationBuilder UseHandfire(this IApplicationBuilder app, string caption)
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
