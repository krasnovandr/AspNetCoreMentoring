using Microsoft.AspNetCore.Identity;

namespace AspNetCoreMentoring.UI
{
    public static class MyIdentityDataInitializer
    {
        public static void SeedData(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            SeedRoles(roleManager);
            SeedUsers(userManager);
        }

        public static void SeedUsers(UserManager<IdentityUser> userManager)
        {
            var adminUserNameAndEmail = "admin@contoso.com";
            if (userManager.FindByNameAsync(adminUserNameAndEmail).Result == null)
            {
                var user = new IdentityUser
                {
                    UserName = adminUserNameAndEmail,
                    Email = adminUserNameAndEmail,
                    EmailConfirmed = true
                };

                IdentityResult result = userManager.CreateAsync(user, "3323876qW").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Administrator").Wait();
                }
            }
        }

        public static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync("Administrator").Result)
            {
                var role = new IdentityRole();
                role.Name = "Administrator";
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }

        }
    }
}
