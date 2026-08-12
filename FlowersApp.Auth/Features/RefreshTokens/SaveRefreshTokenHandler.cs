using FlowersApp.Auth.Infrastructure.Persistence;
using FlowersApp.Auth.Shared.Response;
using FlowersApp.Auth.Domain.Entities;
using FlowersApp.Auth.Shared.Interfaces;
using MediatR;

namespace FlowersApp.Auth.Features.RefreshTokens;

public class SaveRefreshTokenHandler : ICommandHandler<SaveRefreshTokenCommand, Guid>
{
    private readonly AppDbContext _db;

    public SaveRefreshTokenHandler(AppDbContext db)
    {
        _db = db;
    }

    public async Task<RequestResult<Guid>> Handle(SaveRefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var entity = new RefreshToken
        {
            Id = Guid.NewGuid(),
            Token = request.Token,
            ExpiresAt = request.ExpiresAt,
            UserId = request.UserId
        };
        _db.RefreshTokens.Add(entity);
        await _db.SaveChangesAsync(cancellationToken);
        return RequestResult<Guid>.succeeded(entity.Id, ResultCode.RegistrationSuccessful);
    }
}
