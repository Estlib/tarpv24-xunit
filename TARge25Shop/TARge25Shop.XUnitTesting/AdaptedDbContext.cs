using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;


namespace TARge25Shop.XUnitTesting
{
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            return userIdentity;
        }
    }
    public class AdaptedDbContext : IdentityDbContext<ApplicationUser>
    {

    }
}
//https://github.com/AnastasiiaRadasheva/kooli/blob/testimine/Kool/Models/IdentityModels.cs
