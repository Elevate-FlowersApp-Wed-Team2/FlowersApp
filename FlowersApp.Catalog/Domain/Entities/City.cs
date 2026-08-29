namespace FlowersApp.Catalog.Domain.Entities
{
    public class City
    {
        public int Id { get; set; }
        public int GovernorateId { get; set; }
        public Governorate Governorate { get; set; } = default!;
        public string NameAr { get; set; } = default!;
        public string NameEn { get; set; } = default!;
    }
}
