namespace FlowersApp.Cart.Features.Addresses.GetDefaultAddress
{
    public record DefaultAddressDto(
     Guid Id, string Label, string Street, string City, string Governorate,
     double Latitude, double Longitude);
}
