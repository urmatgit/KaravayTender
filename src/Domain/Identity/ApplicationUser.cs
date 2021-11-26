// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

using CleanArchitecture.Razor.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CleanArchitecture.Razor.Domain.Identity
{
    public class ApplicationUser : IdentityUser, IApplicationUser
    {
        public string DisplayName { get; set; }

        [Column(TypeName = "text")]
        public string ProfilePictureDataUrl { get; set; }
        public bool IsActive { get; set; }
        public bool IsLive { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
        [JsonIgnore]
        public virtual ICollection<ApplicationUserClaim> Claims { get; set; }
        [JsonIgnore]
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
        [JsonIgnore]
        public virtual ICollection<ApplicationUserLogin> Logins { get; set; }
        [JsonIgnore]
        public virtual ICollection<ApplicationUserToken> Tokens { get; set; }
        [JsonIgnore]
        public virtual ICollection<Contragent> Contragents { get; set; }
    }
}
