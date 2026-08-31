using FluentValidation;

namespace FlowersApp.Cart.Features.Addresses.SetDefaultAddress
{
    public class SetDefaultAddressValidator : AbstractValidator<SetDefaultAddressCommand>
    {
        public SetDefaultAddressValidator()
        {
            RuleFor(x => x.AddressId).NotEmpty();
        }
    }
}
