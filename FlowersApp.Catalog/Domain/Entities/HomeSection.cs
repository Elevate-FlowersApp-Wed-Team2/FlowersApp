using FlowersApp.Catalog.Domain.Entities;

namespace FloweryApp.Api.Domain.Entities;

public sealed class HomeSection : BaseEntity
{
    public string Type { get; set; }
    public string? Title { get; set; }
    public int Order { get; set; }
    public bool Enabled { get; set; } = true;
    public bool BestSellersOnly { get; set; }
    public int MaxItems { get; set; } = 10;
    public string? BannerImageUrl { get; set; }
    public string? BannerDeepLink { get; set; }
}
