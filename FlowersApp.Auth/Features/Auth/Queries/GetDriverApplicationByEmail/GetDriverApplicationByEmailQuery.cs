using FlowersApp.Auth.Domain.Entities;
using FlowersApp.Auth.Infrastructure.Persistence;
using FlowersApp.Auth.Shared.Interfaces;
using FlowersApp.Auth.Shared.Response;
using Microsoft.EntityFrameworkCore;

namespace FlowersApp.Auth.Features.Auth.Queries.GetDriverApplicationByEmail;

public record GetDriverApplicationByEmailQuery(string Email) : IQuery<DriverApplication?>;

public class GetDriverApplicationByEmailQueryHandler : IQueryHandler<GetDriverApplicationByEmailQuery, DriverApplication?>
{
    private readonly AppDbContext _db;

    public GetDriverApplicationByEmailQueryHandler(AppDbContext db)
    {
        _db = db;
    }

    public async Task<RequestResult<DriverApplication?>> Handle(GetDriverApplicationByEmailQuery request, CancellationToken cancellationToken)
    {
        var application = await _db.Applications
            .FirstOrDefaultAsync(a => a.Email.ToLower() == request.Email.Trim().ToLower(), cancellationToken);

        return RequestResult<DriverApplication?>.succeeded(application, ResultCode.LoginSuccessful);
    }
}
