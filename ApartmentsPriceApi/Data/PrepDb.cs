using Microsoft.EntityFrameworkCore;

namespace ApartmentsPriceApi.Data
{
    public class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<ApartmentsDbContext>());
            }
        }

        private static void SeedData(ApartmentsDbContext apartmentsDbContext)
        {
            try
            {
                apartmentsDbContext.Database.Migrate();
            }
            catch (Exception ex )
            {
                Console.WriteLine($"Could not run migrations: {ex.Message}");
            }
        }
    }
}
