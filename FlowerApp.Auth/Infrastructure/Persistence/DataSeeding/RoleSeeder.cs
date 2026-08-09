using Microsoft.AspNetCore.Identity;

namespace FlowerApp.Auth.Infrastructure.Persistence.DataSeeding
{
    public static class RoleSeeder
    {
        public static async Task SeedAsync(
            RoleManager<IdentityRole<Guid>> roleManager)
        {
            string[] roles =
            {
                "Customer",
                "Admin",
                "Driver"
            };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(
                        new IdentityRole<Guid>(role));
                }
            }
        }
    }
}