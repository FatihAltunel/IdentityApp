using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IdentityApp.Models
{
    public static class IdentitySeedData
    {
        private const string userAdmin = "Admin";
        private const string userAdminPassword = "Admin_123";

        // Kimlik test kullanıcısını oluşturmak için kullanılan metot
        public static async Task IdentityTestUser(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<IdentityContext>();
                var userManager = services.GetRequiredService<UserManager<IdentityUser>>();

                // Bekleyen migration varsa, bunları uygular
                if (context.Database.GetPendingMigrations().Any())
                {
                    await context.Database.MigrateAsync();
                }

                // Kullanıcı adına göre kullanıcıyı arar
                var user = await userManager.FindByNameAsync(userAdmin);

                // Eğer kullanıcı bulunamazsa, yeni bir kullanıcı oluşturur
                if (user == null)
                {
                    user = new IdentityUser
                    {
                        UserName = "Admin",
                        Email = "admin@123.com",
                        PhoneNumber = "312312312",
                    };
                    await userManager.CreateAsync(user, userAdminPassword);
                }
            }
        }
    }
}