using FlowersApp.Cart.Domain.Entities;
using FlowersApp.Cart.Infrastructure.Persistence.Repositories;
using FlowersApp.Cart.Shared.Interfaces;
using FlowersApp.Cart.Shared.Response;
using Microsoft.EntityFrameworkCore;

namespace FlowersApp.Cart.Features.Addresses.GetDefaultAddress
{
    public class GetDefaultAddressHandler : IQueryHandler<GetDefaultAddressQuery, DefaultAddressDto?>
    {
        private readonly Repository<Address> _addressRepository;
        private readonly ICurrentUserService _currentUser;

        public GetDefaultAddressHandler(Repository<Address> addressRepository, ICurrentUserService currentUser)
        {
            _addressRepository = addressRepository;
            _currentUser = currentUser;
        }

        public async Task<RequestResult<DefaultAddressDto?>> Handle(GetDefaultAddressQuery q, CancellationToken ct)
        {
            var userId = Guid.Parse(_currentUser.UserId);

            var defaultAddress = await _addressRepository
                .Get(a => a.UserId == userId && a.IsDefault)
                .Select(a => new DefaultAddressDto(a.Id, a.Label, a.Street, a.City, a.Governorate, a.Latitude, a.Longitude))
                .FirstOrDefaultAsync(ct);

            // no default (including zero addresses) is a normal state, not an error.
            return defaultAddress is null
                ? RequestResult<DefaultAddressDto?>.Failure(ResultCode.NoDefaultAddressFound)
                : RequestResult<DefaultAddressDto?>.succeeded(defaultAddress, ResultCode.DefaultAddressSet);
        }
    }
}
