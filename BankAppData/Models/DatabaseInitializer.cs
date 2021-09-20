using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BankAppData.Models
{
    public class DatabaseInitializer
    {
        public void Initialize(BankAppDataContext context, UserManager<IdentityUser> userManager)
        {

            context.Database.Migrate();
            AddRoleIfNotExists(context, "Admin");
            AddRoleIfNotExists(context, "Cashier");

            AddUserIfNotExists(userManager, "stefan.holmberg@systementor.se", "Admin");
            AddUserIfNotExists(userManager, "Marwan.Saadeh@yh.nackademin.se", "Cashier");


        }



        private void AddRoleIfNotExists(BankAppDataContext context, string role)
        {
            if (context.Roles.Any(r => r.Name == role)) return;
            context.Roles.Add(new IdentityRole { Name = role, NormalizedName = role });
            context.SaveChanges();
        }

        private void AddUserIfNotExists(UserManager<IdentityUser> userManager, string user, string role)
        {
            if (userManager.FindByEmailAsync(user).Result == null)
            {
                var u = new IdentityUser
                {
                    UserName = user,
                    Email = user,
                    EmailConfirmed = true
                };

                var result = userManager.CreateAsync(u, "Hejsan123#").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(u, role).Wait();
                }
            }
        }
    }
}
