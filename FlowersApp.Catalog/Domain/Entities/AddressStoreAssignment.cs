namespace FlowersApp.Catalog.Domain.Entities
{
    public class AddressStoreAssignment : BaseEntity
    {
        public Guid Id { get; set; }

        // Reference into whatever service owns the address (Cart/Order) — no cross-service FK.
        public Guid AddressId { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string? City { get; set; }

        public Guid? StoreId { get; set; }
        public Store? Store { get; set; }

        public bool IsUnresolved { get; set; }
        public DateTime ResolvedAt { get; set; }
    }
}
