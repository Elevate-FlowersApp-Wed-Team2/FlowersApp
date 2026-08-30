using FlowersApp.Cart.Domain.Entities;
using FlowersApp.Cart.Infrastructure.Persistence.Repositories;
using FlowersApp.Cart.Shared.Interfaces;
using FlowersApp.Cart.Shared.Response;
using Microsoft.EntityFrameworkCore;

namespace FlowersApp.Cart.Features.Addresses.SetDefaultAddress
{
    public class SetDefaultAddressHandler : ICommandHandler<SetDefaultAddressCommand, bool>
    {
        private readonly Repository<Address> _addressRepository;
        private readonly ICurrentUserService _currentUser;

        public SetDefaultAddressHandler(Repository<Address> addressRepository, ICurrentUserService currentUser)
        {
            _addressRepository = addressRepository;
            _currentUser = currentUser;
        }

        public async Task<RequestResult<bool>> Handle(SetDefaultAddressCommand cmd, CancellationToken ct)
        {
            if (!Guid.TryParse(_currentUser.UserId, out var userId))
                return RequestResult<bool>.Failure(ResultCode.Unauthorized);

            var target = await _addressRepository
                .Get(a => a.Id == cmd.AddressId)
                .FirstOrDefaultAsync(ct);

            if (target is null)
                return RequestResult<bool>.Failure(ResultCode.AddressNotFound);

            if (target.UserId != userId)
                return RequestResult<bool>.Failure(ResultCode.AddressNotOwned);

            if (target.IsDefault)
                return RequestResult<bool>.succeeded(true, ResultCode.DefaultAddressSet); // already default, no-op

            
            await _addressRepository.BulkUpdateAsync(
                a => a.UserId == userId && a.IsDefault,
                a => a.IsDefault,
                false,
                ct);

            // set the requested address as default.
            target.IsDefault = true;
            _addressRepository.SaveInclude(target, nameof(Address.IsDefault));
            await _addressRepository.SaveChangeAsync(ct);

            return RequestResult<bool>.succeeded(true, ResultCode.DefaultAddressSet);
        }
    }
}
