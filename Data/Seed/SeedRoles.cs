using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyInstagramApi.Data.Seed
{
    public static class SeedRoles
    {
        public static async Task Initlialize(RoleManager<IdentityRole> roleManager)
        {
            List<string> roles = new List<string>() { "User", "Admin", "Administrator", "Owner" };

            foreach(string role in roles)
            {
                if(!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }
    }
}
