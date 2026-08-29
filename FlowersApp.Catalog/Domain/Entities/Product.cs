using FlowersApp.Catalog.Domain.Entities;
using FloweryApp.Api.Domain.Entities;

namespace FlowersApp.Catalog.Domain.Entities
{
    public class Product : BaseEntity
    {
       
        public string Name { get; set; }
        public string Description { get; set; }
        public List<string> ImageUrls { get; set; } = new();
        public List<string> Includes { get; set; } = new();
        public decimal Price { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public int StockQuantity { get; set; }
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }
        public Guid StoreId { get; set; }
        public bool IsActive { get; set; }
        public List<Occasion> Occasions { get; set; } = new();
    }
}
