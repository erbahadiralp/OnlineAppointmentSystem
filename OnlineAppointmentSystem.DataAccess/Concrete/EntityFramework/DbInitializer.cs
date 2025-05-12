using Microsoft.AspNetCore.Identity;
using OnlineAppointmentSystem.Entity.Concrete;
using System;
using System.Threading.Tasks;

namespace OnlineAppointmentSystem.DataAccess.Concrete.EntityFramework
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(OnlineAppointmentSystemDbContext context, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            // Rolleri oluştur
            string[] roleNames = { "Admin", "Employee", "Customer" };
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // Admin kullanıcısını oluştur
            var adminEmail = "admin@example.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new AppUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FirstName = "Admin",
                    LastName = "User",
                    EmailConfirmed = true,
                    IsActive = true,
                    CreatedDate = DateTime.Now
                };

                var result = await userManager.CreateAsync(adminUser, "Admin123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }

            await context.SaveChangesAsync();
        }
    }
} 