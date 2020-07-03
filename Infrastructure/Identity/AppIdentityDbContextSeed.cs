using System.Linq;
using System.Threading.Tasks;
using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedUserAsync(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var user = new AppUser
                {
                    DisplayName = "Marian",
                    Email = "marian@test.com",
                    UserName = "marian@test.com",
                    Adress = new Adress
                    {
                        FirstName = "Marian",
                        LastName = "Kox",
                        Street = "10 Elm Street",
                        City = "Blachownia",
                        State = "Śląskie",
                        Zipcode = "42-290"
                    }
                };

                await userManager.CreateAsync(user,"Pa$$w0rd");
            }
        }
    }
}