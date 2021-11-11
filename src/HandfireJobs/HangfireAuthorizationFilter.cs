// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using CleanArchitecture.Razor.Infrastructure.Constants.Permission;
using Hangfire.Dashboard;
using Microsoft.AspNetCore.Http;

namespace HandfireJobs
{
    public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            //TODO implement authorization logic
            return true;
            var httpContext = context.GetHttpContext();
            string jwtToken = "";
            var read = httpContext.Request.Query.TryGetValue("token", out var jwtTokenFromQuery);
            if (read)
            {
                jwtToken = jwtTokenFromQuery.ToString();
                CookieOptions options = new CookieOptions
                {
                    Expires = DateTime.Now.AddMinutes(60)
                };
                httpContext.Response.Cookies.Append("token", jwtToken, options);
            }
            else
            {
                read = httpContext.Request.Cookies.TryGetValue("token", out jwtToken);
            }

            if (!read) return false;

            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwtToken);
            if (token is null) return false;
            var hangfireViewPermission =
                token.Claims.Any(w => w.Value.Equals(Permissions.Hangfire.View));

            return hangfireViewPermission;
        }
    }
}
