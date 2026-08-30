namespace FlowersApp.Cart.Domain.Entities
{
    public class Address : BaseEntity
    {
        public Guid UserId { get; set; }
        public string Label { get; set; } = default!;
        public string Street { get; set; } = default!;
        public string City { get; set; } = default!;
        public string Governorate { get; set; } = default!;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public bool IsDefault { get; set; }   
    }
}
