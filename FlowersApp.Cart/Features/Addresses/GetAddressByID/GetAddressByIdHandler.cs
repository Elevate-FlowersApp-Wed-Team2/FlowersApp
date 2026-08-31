using FlowersApp.Cart.Domain.Entities;
using FlowersApp.Cart.Infrastructure.Persistence.Repositories;
using FlowersApp.Cart.Shared.Interfaces;
using FlowersApp.Cart.Shared.Response;
using Microsoft.EntityFrameworkCore;

namespace FlowersApp.Cart.Features.Addresses.GetAddressByID
{
    public class GetAddressByIdHandler : IQueryHandler<GetAddressByIdQuery, AddressDetailsDto>
    {
        private readonly Repository<Address> _addressRepository;
        private readonly ICurrentUserService _currentUser;

        public GetAddressByIdHandler(Repository<Address> addressRepository, ICurrentUserService currentUser)
        {
            _addressRepository = addressRepository;
            _currentUser = currentUser;
        }

        public async Task<RequestResult<AddressDetailsDto>> Handle(GetAddressByIdQuery q, CancellationToken ct)
        {
            var userId = Guid.Parse(_currentUser.UserId);

            var address = await _addressRepository
                .Get(a => a.Id == q.Id && a.UserId == userId)
                .Select(a => new AddressDetailsDto(
                    a.Id, a.Label, a.Street, a.City, a.Governorate, a.Latitude, a.Longitude, a.IsDefault))
                .FirstOrDefaultAsync(ct);

            return address is null
                ? RequestResult<AddressDetailsDto>.Failure(ResultCode.AddressNotFound)
                : RequestResult<AddressDetailsDto>.succeeded(address, ResultCode.DefaultAddressSet);
        }
    }
}
