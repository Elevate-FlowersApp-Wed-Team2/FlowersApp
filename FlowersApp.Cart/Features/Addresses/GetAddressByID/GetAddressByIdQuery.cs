using FlowersApp.Cart.Shared.Interfaces;

namespace FlowersApp.Cart.Features.Addresses.GetAddressByID
{
    public record GetAddressByIdQuery(Guid Id) : IQuery<AddressDetailsDto>;
}
