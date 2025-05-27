using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace SportsStore.Models;

public static class IdentitySeedData
{
    private const string adminUser = "Admin";
    private const string adminPassword = "pa$$word";

    public static async void EnsurePopulated(IApplicationBuilder app)
    {
        AppIdentityDbContext context = app.ApplicationServices
            .CreateScope().ServiceProvider
            .GetRequiredService<AppIdentityDbContext>();
        if (context.Database.GetPendingMigrations().Any())
        {
            context.Database.Migrate();
        }

        UserManager<IdentityUser> userManager = app.ApplicationServices
            .CreateScope().ServiceProvider
            .GetRequiredService<UserManager<IdentityUser>>();

        IdentityUser user = await userManager.FindByNameAsync(adminUser);
        if (user == null)
        {
            user = new IdentityUser("Admin");
            user.Email = "admin@admin.com";
            user.PhoneNumber = "123-456";
            await userManager.CreateAsync(user, adminPassword);
        }
    }
}