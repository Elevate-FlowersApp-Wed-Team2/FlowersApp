using FlowersApp.Auth.Domain.Entities;

namespace FlowersApp.Auth.Shared.Interfaces;

public interface ITokenService
{
    Task<(string AccessToken, DateTime ExpiresAt)> GenerateAccessTokenAsync(AppUser user, string role, CancellationToken cancellationToken = default);
    Task<RefreshToken> GenerateRefreshTokenAsync(Guid userId, CancellationToken cancellationToken = default);
}
