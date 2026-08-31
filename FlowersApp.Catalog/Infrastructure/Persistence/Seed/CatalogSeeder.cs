using FlowersApp.Catalog.Domain.Entities;
using FlowersApp.Catalog.Domain.Enum;
using FloweryApp.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FlowersApp.Catalog.Infrastructure.Persistence.Seed
{
    public static class CatalogSeeder
    {
        public static async Task SeedAsync(CatalogDbContext context)
        {
            await SeedCategoriesAsync(context);
            await SeedOccasionsAsync(context);
            await SeedProductsAsync(context);
        }

        private static async Task SeedCategoriesAsync(CatalogDbContext context)
        {
            if (await context.Categories.AnyAsync()) return;

            context.Categories.AddRange(
                new Category { Name = "Roses", IconUrl = "https://example.com/categories/roses.png", IsActive = true, SortOrder = 1, CreatedBy = "seed" },
                new Category { Name = "Bouquets", IconUrl = "https://example.com/categories/bouquets.png", IsActive = true, SortOrder = 2, CreatedBy = "seed" },
                new Category { Name = "Plants", IconUrl = "https://example.com/categories/plants.png", IsActive = true, SortOrder = 3, CreatedBy = "seed" },
                new Category { Name = "Discontinued Category", IconUrl = "https://example.com/categories/old.png", IsActive = false, SortOrder = 4, CreatedBy = "seed" }
            );

            await context.SaveChangesAsync();
        }

        private static async Task SeedOccasionsAsync(CatalogDbContext context)
        {
            if (await context.Occasions.AnyAsync()) return;

            context.Occasions.AddRange(
                new Occasion { Name = "Wedding", ImageUrl = "https://example.com/occasions/wedding.jpg", IsActive = true, SortOrder = 1, CreatedBy = "seed" },
                new Occasion { Name = "Birthday", ImageUrl = "https://example.com/occasions/birthday.jpg", IsActive = true, SortOrder = 2, CreatedBy = "seed" },
                new Occasion { Name = "Get Well Soon", ImageUrl = "https://example.com/occasions/get-well-soon.jpg", IsActive = true, SortOrder = 3, CreatedBy = "seed" },
                new Occasion { Name = "Graduation", ImageUrl = "https://example.com/occasions/graduation.jpg", IsActive = true, SortOrder = 4, CreatedBy = "seed" },
                new Occasion { Name = "Anniversary", ImageUrl = "https://example.com/occasions/anniversary.jpg", IsActive = true, SortOrder = 5, CreatedBy = "seed" },
                new Occasion { Name = "Discontinued Occasion", ImageUrl = "https://example.com/occasions/old.jpg", IsActive = false, SortOrder = 6, CreatedBy = "seed" }
            );

            await context.SaveChangesAsync();
        }

        private static async Task SeedProductsAsync(CatalogDbContext context)
        {
            if (await context.Products.AnyAsync()) return;

            var rosesCat = await context.Categories.FirstAsync(c => c.Name == "Roses");
            var bouquetsCat = await context.Categories.FirstAsync(c => c.Name == "Bouquets");
            var plantsCat = await context.Categories.FirstAsync(c => c.Name == "Plants");
            var oldCat = await context.Categories.FirstAsync(c => c.Name == "Discontinued Category");

            var wedding = await context.Occasions.FirstAsync(o => o.Name == "Wedding");
            var birthday = await context.Occasions.FirstAsync(o => o.Name == "Birthday");
            var getWell = await context.Occasions.FirstAsync(o => o.Name == "Get Well Soon");
            var graduation = await context.Occasions.FirstAsync(o => o.Name == "Graduation");
            var anniversary = await context.Occasions.FirstAsync(o => o.Name == "Anniversary");

            var products = new List<Product>
            {
                new Product
                {
                    Name = "Red Rose Bouquet",
                    Description = "A classic dozen red roses, hand-tied with elegant ribbon.",
                    ImageUrls = new List<string> { "https://example.com/products/red-roses-1.jpg", "https://example.com/products/red-roses-2.jpg" },
                    Includes = new List<string> { "12 red roses", "Greenery filler", "Wrapping paper" },
                    Price = 350.00m,
                    DiscountPercentage = 20,
                    StockQuantity = 15,
                    CategoryId = rosesCat.Id,
                    IsActive = true,
                    CreatedBy = "seed",
                    Occasions = new List<Occasion> { anniversary, birthday }
                },
                new Product
                {
                    Name = "White Elegance Arrangement",
                    Description = "Pure white roses paired with delicate gypsophila.",
                    ImageUrls = new List<string> { "https://example.com/products/white-roses.jpg" },
                    Includes = new List<string> { "15 white roses", "Gypsophila", "Glass vase" },
                    Price = 500.00m,
                    DiscountPercentage = 10,
                    StockQuantity = 8,
                    CategoryId = rosesCat.Id,
                    IsActive = true,
                    CreatedBy = "seed",
                    Occasions = new List<Occasion> { wedding, anniversary }
                },
                new Product
                {
                    Name = "Sunflower Basket",
                    Description = "Bright sunflowers arranged in a rustic woven basket.",
                    ImageUrls = new List<string> { "https://example.com/products/sunflowers-1.jpg" },
                    Includes = new List<string> { "8 sunflowers", "Woven basket", "Yellow ribbon" },
                    Price = 280.00m,
                    DiscountPercentage = 0,
                    StockQuantity = 0,
                    CategoryId = bouquetsCat.Id,
                    IsActive = true,
                    CreatedBy = "seed",
                    Occasions = new List<Occasion> { getWell, graduation }
                },
                new Product
                {
                    Name = "Spring Meadow Mix",
                    Description = "A vibrant mix of tulips, daisies, and seasonal greenery.",
                    ImageUrls = new List<string> { "https://example.com/products/spring-mix.jpg" },
                    Includes = new List<string> { "Mixed tulips", "Daisies", "Craft paper wrap" },
                    Price = 420.00m,
                    DiscountPercentage = 15,
                    StockQuantity = 25,
                    CategoryId = bouquetsCat.Id,
                    IsActive = true,
                    CreatedBy = "seed",
                    Occasions = new List<Occasion> { birthday, graduation }
                },
                new Product
                {
                    Name = "Peace Lily Potted Plant",
                    Description = "Air-purifying indoor Peace Lily in a minimal ceramic pot.",
                    ImageUrls = new List<string> { "https://example.com/products/peace-lily.jpg" },
                    Includes = new List<string> { "Peace Lily plant", "Ceramic pot", "Care instructions card" },
                    Price = 220.00m,
                    DiscountPercentage = 0,
                    StockQuantity = 12,
                    CategoryId = plantsCat.Id,
                    IsActive = true,
                    CreatedBy = "seed",
                    Occasions = new List<Occasion> { getWell }
                },
                new Product
                {
                    Name = "Monstera Deliciosa",
                    Description = "Popular Swiss Cheese Plant with large, glossy green leaves.",
                    ImageUrls = new List<string> { "https://example.com/products/monstera.jpg" },
                    Includes = new List<string> { "Monstera plant", "Terracotta pot" },
                    Price = 310.00m,
                    DiscountPercentage = 5,
                    StockQuantity = 5,
                    CategoryId = plantsCat.Id,
                    IsActive = true,
                    CreatedBy = "seed",
                    Occasions = new List<Occasion>()
                },
                new Product
                {
                    Name = "Vintage Dried Lavender Bouquet",
                    Description = "Fragrant dried French lavender stems in rustic packaging.",
                    ImageUrls = new List<string> { "https://example.com/products/lavender.jpg" },
                    Includes = new List<string> { "Dried lavender stems", "Jute twine wrap" },
                    Price = 180.00m,
                    DiscountPercentage = 0,
                    StockQuantity = 0,
                    CategoryId = oldCat.Id,
                    IsActive = false,
                    CreatedBy = "seed",
                    Occasions = new List<Occasion>()
                }
            };

            var allOccasions = products.SelectMany(p => p.Occasions).Distinct();
            foreach (var occ in allOccasions)
            {
                context.Entry(occ).State = EntityState.Unchanged;
            }

            await context.Products.AddRangeAsync(products);
            await context.SaveChangesAsync();
        }
    }

}
