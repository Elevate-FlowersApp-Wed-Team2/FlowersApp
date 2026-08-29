namespace FlowersApp.Catalog.Domain.Entities
{
    public class Governorate
    {
        public int Id { get; set; }
        public string NameAr { get; set; } = default!;
        public string NameEn { get; set; } = default!;
        public ICollection<City> Cities { get; set; } = new List<City>();
    }
}
