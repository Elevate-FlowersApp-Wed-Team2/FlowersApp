using CustomerEntity = FlowersApp.Auth.Domain.Entities.Customer;
using FlowersApp.Auth.Domain.Entities;
using FlowersApp.Auth.Shared.Interfaces;
using FlowersApp.Auth.Shared.Response;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FlowersApp.Auth.Features.Auth.Queries.GetCustomerByEmail;

public record GetCustomerByEmailQuery(string Email) : IQuery<CustomerEntity?>;

public class GetCustomerByEmailQueryHandler : IQueryHandler<GetCustomerByEmailQuery, CustomerEntity?>
{
    private readonly UserManager<AppUser> _userManager;

    public GetCustomerByEmailQueryHandler(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<RequestResult<CustomerEntity?>> Handle(GetCustomerByEmailQuery request, CancellationToken cancellationToken)
    {
        var normalizedEmail = _userManager.NormalizeEmail(request.Email);
        var customer = await _userManager.Users
            .OfType<CustomerEntity>()
            .FirstOrDefaultAsync(c => c.NormalizedEmail == normalizedEmail, cancellationToken);

        return RequestResult<CustomerEntity?>.succeeded(customer, ResultCode.LoginSuccessful);
    }
}
