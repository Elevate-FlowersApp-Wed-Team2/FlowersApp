using FlowersApp.Auth.Domain.Entities;
using FlowersApp.Auth.Shared.Constants;
using FlowersApp.Auth.Shared.Interfaces;
using FlowersApp.Auth.Shared.Response;

namespace FlowersApp.Auth.Features.Auth;

internal static class AuthTokenIssuer
{
    public static async Task<AuthResponse> IssueAsync(
        ITokenService tokenService,
        AppUser user,
        string role,
        string? applicationStatus = null,
        string? deviceInfo = null,
        string? ipAddress = null,
        CancellationToken cancellationToken = default)
    {
        var access = await tokenService.GenerateAccessTokenAsync(
            user, role, applicationStatus, cancellationToken);

        var refresh = await tokenService.GenerateRefreshTokenAsync(
            user.Id,
            familyId: null,
            deviceInfo,
            ipAddress,
            cancellationToken);

        return new AuthResponse(
            access.AccessToken,
            refresh.RawToken,
            access.ExpiresInSeconds,
            role,
            applicationStatus);
    }

    public static AuthResponse EmptyWithStatus(string role, string driverStatus) =>
        new(
            AccessToken: string.Empty,
            RefreshToken: string.Empty,
            ExpiresIn: 0,
            Role: role,
            DriverStatus: driverStatus);
}
