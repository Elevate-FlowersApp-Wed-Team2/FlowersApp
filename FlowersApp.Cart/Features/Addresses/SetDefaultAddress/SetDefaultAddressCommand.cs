using FlowersApp.Cart.Shared.Interfaces;

namespace FlowersApp.Cart.Features.Addresses.SetDefaultAddress
{
    public record SetDefaultAddressCommand(Guid AddressId) : ICommand<bool>;
}
