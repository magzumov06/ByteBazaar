using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Seeder;

public static class Seed
{
    public static async Task SeedAdmin(UserManager<User> userManager, RoleManager<IdentityRole<int>> roleManager)
    {
        if (!roleManager.RoleExistsAsync(Role.Admin.ToString()).Result)
        {
            await roleManager.CreateAsync(new IdentityRole<int>("Admin"));
        }
        var user = userManager.Users.FirstOrDefault(x=> x.UserName == "Admin");
        if (user == null)
        {
            var newUser = new User()
            {
                FullName = "Admin",
                UserName = "Admin",
                Address = "Dushanbe",
                Email = "admin@gmail.com",
                PhoneNumber = "987654321",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };
            var res = userManager.CreateAsync(newUser, "zxcv0987?");
            if (res.Result.Succeeded)
            {
                await userManager.AddToRoleAsync(newUser, Role.Admin.ToString());
            }
        }
    }

    public static async Task<bool> SeedRole(RoleManager<IdentityRole<int>> roleManager)
    {
        var newRole = new List<IdentityRole<int>>()
        {
            new(Role.Admin.ToString()),
            new(Role.Customer.ToString()),
        };
        var roles = await roleManager.Roles.ToListAsync();
        foreach (var role in newRole)
        {
            if(roles.Any(r=>r.Name == role.Name))
                continue;
            await roleManager.CreateAsync(role);
        }
        return true;
    }

}