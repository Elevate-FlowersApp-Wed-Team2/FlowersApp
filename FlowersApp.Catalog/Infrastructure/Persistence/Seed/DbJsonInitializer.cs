using FlowersApp.Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FlowersApp.Catalog.Infrastructure.Persistence.Seed
{
    public static class DbJsonInitializer
    {
        public static async Task SeedDataAsync(DbContext dbContext, string jsonFolderPath)
        {
            await dbContext.Database.EnsureCreatedAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                NumberHandling = JsonNumberHandling.AllowReadingFromString
            };

            if (!await dbContext.Set<Governorate>().AnyAsync())
            {
                var path = Path.Combine(jsonFolderPath, "cities.json");
                if (File.Exists(path))
                {
                    var json = await File.ReadAllTextAsync(path);
                    var root = JsonSerializer.Deserialize<List<JsonTypeWrapper<GovernorateDto>>>(json, options);

                    var data = root?.FirstOrDefault(x => x.Type == "table")?.Data;
                    if (data != null && data.Any())
                    {
                        var entities = data.Select(x => new Governorate
                        {
                            Id = x.Id,
                            NameAr = x.GovernorateNameAr,
                            NameEn = x.GovernorateNameEn
                        });

                        await dbContext.Set<Governorate>().AddRangeAsync(entities);
                        await dbContext.SaveChangesAsync();
                    }
                }
            }

            if (!await dbContext.Set<City>().AnyAsync())
            {
                var path = Path.Combine(jsonFolderPath, "states.json");
                if (File.Exists(path))
                {
                    var json = await File.ReadAllTextAsync(path);
                    var root = JsonSerializer.Deserialize<List<JsonTypeWrapper<CityDto>>>(json, options);

                    var data = root?.FirstOrDefault(x => x.Type == "table")?.Data;
                    if (data != null && data.Any())
                    {
                        var entities = data.Select(x => new City
                        {
                            Id = x.Id,
                            GovernorateId = x.GovernorateId,
                            NameAr = x.CityNameAr,
                            NameEn = x.CityNameEn
                        });

                        await dbContext.Set<City>().AddRangeAsync(entities);
                        await dbContext.SaveChangesAsync();
                    }
                }
            }
        }

        private class JsonTypeWrapper<T>
        {
            public string Type { get; set; } = string.Empty;
            public List<T>? Data { get; set; }
        }

        private class GovernorateDto
        {
            public int Id { get; set; }

            [JsonPropertyName("governorate_name_ar")]
            public string GovernorateNameAr { get; set; } = string.Empty;

            [JsonPropertyName("governorate_name_en")]
            public string GovernorateNameEn { get; set; } = string.Empty;
        }

        private class CityDto
        {
            public int Id { get; set; }

            [JsonPropertyName("governorate_id")]
            public int GovernorateId { get; set; }

            [JsonPropertyName("city_name_ar")]
            public string CityNameAr { get; set; } = string.Empty;

            [JsonPropertyName("city_name_en")]
            public string CityNameEn { get; set; } = string.Empty;
        }
    }
}
            