using Microsoft.EntityFrameworkCore;

namespace SportsStore.Models;

public static class SeedData
{
    public static void EnsurePopulated(IApplicationBuilder app)
    {
        StoreDbContext context = app.ApplicationServices
            .CreateScope().ServiceProvider.GetRequiredService<StoreDbContext>();
        if (context.Database.GetPendingMigrations().Any())
        {
            context.Database.Migrate();
        }

        if (!context.Products.Any())
        {
            context.Products.AddRange(
                new Product
                {
                    Name = "name test 1",
                    Description = "description test 1",
                    Category = "category test 1", Price = 94
                },
                new Product
                {
                    Name = "name test 2",
                    Description = "description test 2",
                    Category = "category test 1", Price = 31.15m
                },
                new Product
                {
                    Name = "name test 3",
                    Description = "description test 3",
                    Category = "category test 2", Price = 7.10m
                },
                new Product
                {
                    Name = "name test 4",
                    Description = "description test 4",
                    Category = "category test 2", Price = 47.70m
                },
                new Product
                {
                    Name = "name test 5",
                    Description = "description test 5",
                    Category = "category test 2", Price = 25.30m
                },
                new Product
                {
                    Name = "name test 6",
                    Description = "description test 6",
                    Category = "category test 3", Price = 8.60m
                },
                new Product
                {
                    Name = "name test 7",
                    Description = "description test 7",
                    Category = "category test 3", Price = 9.65m
                },
                new Product
                {
                    Name = "name test 8",
                    Description = "description test 8",
                    Category = "category test 3", Price = 23.55m
                },
                new Product
                {
                    Name = "name test 9",
                    Description = "description test 9",
                    Category = "category test 3", Price = 560
                }
            );
            context.SaveChanges();
        }
    }
}