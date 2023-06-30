using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanEjdg.Infrastructure.SeedData
{
    public static class RoleSeedData
    {
        public static async Task SeedDataBase(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            if (roleManager.Roles.ToArray().Length == 0)
            {
                IdentityRole catEditorRole = new IdentityRole { Name = "CatEditor" };
                IdentityResult result = await roleManager.CreateAsync(catEditorRole);

                IdentityRole adminRole = new IdentityRole { Name = "Admin" };
                result = await roleManager.CreateAsync(adminRole);

                IdentityRole itemEditorRole = new IdentityRole { Name = "ItemEditor" };
                result = await roleManager.CreateAsync(itemEditorRole);

                IdentityRole orderEditorRole = new IdentityRole { Name = "OrderEditor" };
                result = await roleManager.CreateAsync(orderEditorRole);
            }
            IdentityUser user = await userManager.FindByNameAsync("Admin");
            if (user == null)
            {
                IdentityUser admin = new IdentityUser { UserName = "Admin", Email = "admin@admin.org" };
                await userManager.CreateAsync(admin, "Secret123$");
                await userManager.AddToRoleAsync(admin, "Admin");
            }
        }
    }
}
