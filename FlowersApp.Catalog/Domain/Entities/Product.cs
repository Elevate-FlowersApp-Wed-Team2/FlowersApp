using FlowersApp.Catalog.Domain.Entities;

namespace FlowersApp.Catalog.Domain.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
        public decimal? DiscountPercentage { get; set; } // null/0 = no active discount
        public int StockQuantity { get; set; }
        public Guid StoreId { get; set; }
        public Guid? CategoryId { get; set; }
        public bool IsActive { get; set; }
    }
}
