using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineAuction.WebUI.Models;

namespace OnlineAuction.WebUI.Data
{
    public class WebAppContextSeed
    {
        public static async Task SeedAsync(WebAppContext webAppContext, ILoggerFactory loggerFactory, int? retry = 0)
        {
            int retryForAvailability = retry.Value;

            try
            {
                webAppContext.Database.Migrate();
                if (!webAppContext.AppUsers.Any())
                {
                    webAppContext.AppUsers.AddRange(GetPreconfiguredOrders());
                    await webAppContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                if (retryForAvailability < 50)
                {
                    retryForAvailability++;
                    var log = loggerFactory.CreateLogger<WebAppContextSeed>();
                    log.LogError(ex.Message);
                    Thread.Sleep(2000);
                    await SeedAsync(webAppContext, loggerFactory, retryForAvailability);
                }
            }
        }

        private static IEnumerable<AppUser> GetPreconfiguredOrders()
        {
            var hasher = new PasswordHasher<AppUser>();
            var list = new List<AppUser>()
            {
                new AppUser
                {
                    FirstName ="test",
                    LastName = "user",
                    IsSeller = true,
                    IsBuyer = false,
                    Email="test@example.com",
                    NormalizedEmail = "TEST@EXAMPLE.COM",
                    UserName="test@example.com"
                }
            };
            list.ForEach(x => x.PasswordHash = hasher.HashPassword(x, x.FirstName));
            return list;

        }
    }
}
