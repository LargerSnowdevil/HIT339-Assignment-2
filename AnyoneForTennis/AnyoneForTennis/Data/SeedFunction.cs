using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnyoneForTennis.Data
{
    public class SeedFunction
    {
        public async static void Initialize(IServiceProvider serviceProvider)
        {
            RoleManager<IdentityRole> roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            if (roleManager.Roles.Any())
            {
                return;
            }

            var adminIdentityRole = new IdentityRole { Name = "Admin" };
            var memberIdentityRole = new IdentityRole { Name = "Member" };
            var coachIdentityRole = new IdentityRole { Name = "Coach" };

            await roleManager.CreateAsync(adminIdentityRole);
            await roleManager.CreateAsync(memberIdentityRole);
            await roleManager.CreateAsync(coachIdentityRole);
        }
    }
}
