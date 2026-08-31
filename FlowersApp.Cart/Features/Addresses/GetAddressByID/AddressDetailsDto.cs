namespace FlowersApp.Cart.Features.Addresses.GetAddressByID
{
    public record AddressDetailsDto(
     Guid Id,
     string Label,
     string Street,
     string City,
     string Governorate,
     double Latitude,
     double Longitude,
     bool IsDefault);
}
