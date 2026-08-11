using FlowersApp.Auth.Domain.Entities;
using FlowersApp.Auth.Shared.Interfaces;
using FlowersApp.Auth.Shared.Response;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FlowersApp.Auth.Features.Auth.Queries.GetDriverUserByEmail;

public record GetDriverUserByEmailQuery(string Email) : IQuery<Driver?>;

public class GetDriverUserByEmailQueryHandler : IQueryHandler<GetDriverUserByEmailQuery, Driver?>
{
    private readonly UserManager<AppUser> _userManager;

    public GetDriverUserByEmailQueryHandler(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<RequestResult<Driver?>> Handle(GetDriverUserByEmailQuery request, CancellationToken cancellationToken)
    {
        var normalizedEmail = request.Email.Trim().ToUpperInvariant();
        var driver = await _userManager.Users
            .OfType<Driver>()
            .FirstOrDefaultAsync(d => d.NormalizedEmail == normalizedEmail, cancellationToken);

        return RequestResult<Driver?>.succeeded(driver, ResultCode.LoginSuccessful);
    }
}
