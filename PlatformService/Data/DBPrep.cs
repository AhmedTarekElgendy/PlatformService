using Microsoft.EntityFrameworkCore;
using PlatformService.Models;

namespace PlatformService.Data
{
    public static class DBPrep
    {
        public static void PopulateDB(IApplicationBuilder app, bool isDevelopment)
        {
            using(var scope = app.ApplicationServices.CreateScope())
            {
                SeedData(scope.ServiceProvider.GetService<PlatformDBContext>(), isDevelopment);
            }
        }

        private static void SeedData(PlatformDBContext platformDBContext, bool isDevelopment)
        {
            if (!isDevelopment)
                platformDBContext.Database.Migrate();

            if (!platformDBContext.Platforms.Any())
            {
                platformDBContext.Platforms.AddRange(
                    new Platform { Name = ".Net", Publisher = "Microsoft", Cost = "1.2M" },
                    new Platform { Name = "Java", Publisher = "Oracle", Cost = "2.2M" },
                    new Platform { Name = "React", Publisher = "Facebook", Cost = "0.5M" }
                    );

                platformDBContext.SaveChanges();
                Console.WriteLine("Seeding data");
            }
            else
                Console.WriteLine("Data is ready to use");
        }
    }
}
