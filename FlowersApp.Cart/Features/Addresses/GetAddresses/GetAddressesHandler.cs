using FlowersApp.Cart.Domain.Entities;
using FlowersApp.Cart.Infrastructure.Persistence.Repositories;
using FlowersApp.Cart.Shared.Interfaces;
using FlowersApp.Cart.Shared.Response;
using Microsoft.EntityFrameworkCore;

namespace FlowersApp.Cart.Features.Addresses.GetAddresses
{
    public class GetAddressesHandler : IQueryHandler<GetAddressesQuery, List<AddressListItemDto>>
    {
        private readonly Repository<Address> _addressRepository;
        private readonly ICurrentUserService _currentUser;

        public GetAddressesHandler(Repository<Address> addressRepository, ICurrentUserService currentUser)
        {
            _addressRepository = addressRepository;
            _currentUser = currentUser;
        }

        public async Task<RequestResult<List<AddressListItemDto>>> Handle(GetAddressesQuery q, CancellationToken ct)
        {
            var userId = Guid.Parse(_currentUser.UserId);

            var addresses = await _addressRepository
                .Get(a => a.UserId == userId)
                .OrderByDescending(a => a.IsDefault)
                .ThenByDescending(a => a.CreatedAt)
                .Select(a => new AddressListItemDto(a.Id, a.Label, a.Street, a.City, a.Governorate, a.IsDefault))
                .ToListAsync(ct);

            return RequestResult<List<AddressListItemDto>>.succeeded(addresses, ResultCode.DefaultAddressSet);
        }
    }
}
