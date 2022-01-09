// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Linq;
using System.Reflection;
using CleanArchitecture.Razor.Application.Common.Interfaces;
using CleanArchitecture.Razor.Application.Common.Interfaces.Identity;
using CleanArchitecture.Razor.Application.Settings;
using CleanArchitecture.Razor.Infrastructure.Configurations;
using CleanArchitecture.Razor.Infrastructure.Constants.ClaimTypes;
using CleanArchitecture.Razor.Infrastructure.Constants.Localization;
using CleanArchitecture.Razor.Application.Constants.Permission;
using CleanArchitecture.Razor.Domain.Identity;
using CleanArchitecture.Razor.Infrastructure.Localization;
using CleanArchitecture.Razor.Infrastructure.Middlewares;
using CleanArchitecture.Razor.Infrastructure.Persistence;
using CleanArchitecture.Razor.Infrastructure.Services;
using CleanArchitecture.Razor.Infrastructure.Services.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using FluentValidation.AspNetCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace CleanArchitecture.Razor.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseInMemoryDatabase("CleanArchitecture.RazorDb")
                    ); 
            }
            else if (configuration.GetValue<bool>("UseSqliteDatabase"))
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlite(
                        configuration.GetConnectionString("DefaultConnectionSqlite"),
                        b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName))

                    );
            }if (configuration.GetValue<bool>("UsePostgreDatabase"))
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseNpgsql(
                        configuration.GetConnectionString("DefaultConnectionPosgre"),
                        b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName))

                    );
            }
            else
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(
                        configuration.GetConnectionString("DefaultConnection"),
                        b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName))

                    );
                    services.AddDatabaseDeveloperPageExceptionFilter();
            }
            services.Configure<CookiePolicyOptions>(options =>
        {
            // This lambda determines whether user consent for non-essential cookies is needed for a given request.
            options.CheckConsentNeeded = context => true;
            options.MinimumSameSitePolicy = SameSiteMode.None;
        });
            
            services.Configure<SmartSettings>(configuration.GetSection(SmartSettings.SectionName));
            services.AddSingleton(s => s.GetRequiredService<IOptions<SmartSettings>>().Value);
            services.AddSingleton<ICurrentUserService, CurrentUserService>();
            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());
            services.AddScoped<IDomainEventService, DomainEventService>();


            services
                .AddDefaultIdentity<ApplicationUser>()
                .AddRoles<ApplicationRole>()
                .AddErrorDescriber<CustomIdentityErrorDescriber>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddTransient<IDateTime, DateTimeService>();
            services.AddTransient<IExcelService, ExcelService>();
            services.AddTransient<IUploadService, UploadService>();
            services.AddTransient<IIdentityService, IdentityService>();
            services.Configure<AppConfigurationSettings>(configuration.GetSection("AppConfigurationSettings"));
            services.Configure<MailSettings>(configuration.GetSection("MailSettings"));
            services.AddTransient<IMailService, SMTPMailService>();
            services.AddTransient<IDictionaryService, DictionaryService>();
            services.AddAuthentication();
            services.Configure<IdentityOptions>(options =>
            {
                // Default SignIn settings.
                options.SignIn.RequireConfirmedEmail = true;
                options.SignIn.RequireConfirmedPhoneNumber = false;
                //options.User.RequireUniqueEmail = true;

                // Default Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;
            });
            services.Configure<DataProtectionTokenProviderOptions>(opt =>
                    opt.TokenLifespan = TimeSpan.FromHours(2));
            services.AddAuthorization(options =>
            {
                options.AddPolicy("CanPurge", policy => policy.RequireRole("Administrator"));
                // Here I stored necessary permissions/roles in a constant
                foreach (var prop in typeof(Permissions).GetNestedTypes().SelectMany(c => c.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)))
                {
                    var propertyValue = prop.GetValue(null);
                    if (propertyValue is not null)
                    {
                        options.AddPolicy(propertyValue.ToString(), policy => policy.RequireClaim(ApplicationClaimTypes.Permission, propertyValue.ToString()));
                    }
                }
            });
            services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, ApplicationClaimsIdentityFactory>();
            // Localization
            services.AddLocalization(options => options.ResourcesPath = LocalizationConstants.ResourcesPath);
            services.AddScoped<LocalizationCookiesMiddleware>();
             services.AddScoped<ExceptionMiddleware>();
            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.AddSupportedUICultures(LocalizationConstants.SupportedLanguages.Select(x => x.Code).ToArray());
                options.FallBackToParentUICultures = true;
            });
             services.AddControllers();
        services.AddSignalR();
        services.AddRazorPages(options =>
                 {
                     options.Conventions.AddPageRoute("/Karavay/Welcome", "");
                 })
                .AddFluentValidation(fv =>
                 {
                     fv.DisableDataAnnotationsValidation = true;
                     fv.ImplicitlyValidateChildProperties = true;
                     fv.ImplicitlyValidateRootCollectionElements = true;
                 })
                 .AddViewLocalization()
                 .AddNewtonsoftJson(options =>
                 {
                     
                     options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                     var resolver = options.SerializerSettings.ContractResolver as DefaultContractResolver;
                     resolver.NamingStrategy = null;

                 }).AddRazorRuntimeCompilation();
           services.ConfigureApplicationCookie(options => {
            options.LoginPath = "/Identity/Account/Login";
            options.LogoutPath = "/Identity/Account/Logout";
            options.AccessDeniedPath = "/Identity/Account/AccessDenied";
        });
            return services;
        }


    }
}
