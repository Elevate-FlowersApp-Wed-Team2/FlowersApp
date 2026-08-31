using FlowersApp.Cart.Shared.Interfaces;

namespace FlowersApp.Cart.Features.Addresses.GetDefaultAddress
{
    public record GetDefaultAddressQuery : IQuery<DefaultAddressDto?>;
}
