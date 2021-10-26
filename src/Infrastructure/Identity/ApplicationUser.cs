using CleanArchitecture.Razor.Application.Common.Interfaces.Identity.DTOs;
using CleanArchitecture.Razor.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CleanArchitecture.Razor.Infrastructure.Identity
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
        public virtual ICollection<ApplicationUserClaim> Claims { get; set; }
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
        public virtual ICollection<ApplicationUserLogin> Logins { get; set; }
        public virtual ICollection<ApplicationUserToken> Tokens { get; set; }
        public virtual ICollection<Contragent> Contragents { get; set; }
    }
}
