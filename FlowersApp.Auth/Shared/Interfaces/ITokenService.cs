using FlowersApp.Auth.Domain.Entities;

namespace FlowersApp.Auth.Shared.Interfaces;

public record AccessTokenResult(string AccessToken, DateTime ExpiresAt, int ExpiresInSeconds);

public record RefreshTokenIssueResult(string RawToken, RefreshToken Entity);

public interface ITokenService
{
    Task<AccessTokenResult> GenerateAccessTokenAsync(
        AppUser user,
        string role,
        string? applicationStatus = null,
        CancellationToken cancellationToken = default);

    Task<RefreshTokenIssueResult> GenerateRefreshTokenAsync(
        Guid userId,
        Guid? familyId = null,
        string? deviceInfo = null,
        string? ipAddress = null,
        CancellationToken cancellationToken = default);

    string HashToken(string rawToken);
}
