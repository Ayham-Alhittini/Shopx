using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shopx.API.Entities;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Shopx.API.Data
{
    public class Seed
    {
        public static async Task SeedUsers(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (await userManager.Users.AnyAsync()) return;


            /////Seed Roles
            var roles = new List<IdentityRole>
            {
                new IdentityRole{Name = RoleNames.Customer},
                new IdentityRole{Name = RoleNames.Seller},
                new IdentityRole{Name = RoleNames.Admin}
            };
            foreach (var role in roles)
            {
                await roleManager.CreateAsync(role);
            }


            ///Seed Admin
            var admin = new AppUser
            {
                UserName = "admin",
                Email = "shopx.management@gmail.com",
                EmailConfirmed = true,
                AccountState = "active",
                AccountType = "Admin",
            };
            await userManager.CreateAsync(admin, "Pa$$w0rd");
            await userManager.AddToRoleAsync(admin, "Admin");

        }
    }
}
