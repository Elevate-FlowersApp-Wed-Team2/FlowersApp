namespace FlowersApp.Cart.Features.Addresses.GetAddresses
{
    public record AddressListItemDto
        (Guid Id, string Label, string Street, string City, string Governorate, bool IsDefault);

}
