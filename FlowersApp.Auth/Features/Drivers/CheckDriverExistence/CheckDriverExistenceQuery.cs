using FlowersApp.Auth.Domain.Entities;
using FlowersApp.Auth.Infrastructure.Persistence.Repositories;
using FlowersApp.Auth.Shared.Interfaces;
using FlowersApp.Auth.Shared.Response;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FlowersApp.Auth.Features.Drivers.CheckDriverExistence;

public record CheckDriverExistenceQuery(string Email, string NationalIdNumber)
    :IQuery<bool>;

public class CheckIsDriverExistQueryHandler(UserManager<AppUser> userManager) : IQueryHandler<CheckDriverExistenceQuery, bool>
{
    private readonly UserManager<AppUser> _userManager = userManager;


    public async Task<RequestResult<bool>> Handle(CheckDriverExistenceQuery request, CancellationToken cancellationToken)
    {
        var isExist = await _userManager.Users
                               .OfType<Driver>()
                               .AnyAsync(d => d.Email == request.Email
                                       || d.NationalIDNumber == request.NationalIdNumber,
                               cancellationToken);
        if(isExist)
          return RequestResult<bool>.succeeded(isExist ,ResultCode.DriverIsAlreadyExist);
        return RequestResult<bool>.succeeded(isExist ,ResultCode.DriverNotFound);
    }
}

