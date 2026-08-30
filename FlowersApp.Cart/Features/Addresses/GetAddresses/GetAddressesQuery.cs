using FlowersApp.Cart.Shared.Interfaces;

namespace FlowersApp.Cart.Features.Addresses.GetAddresses
{
    public record GetAddressesQuery : IQuery<List<AddressListItemDto>>;
}
