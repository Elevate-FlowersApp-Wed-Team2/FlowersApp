using FlowersApp.Auth.Domain.Entities;
using FlowersApp.Auth.Shared.Interfaces;
using FlowersApp.Auth.Shared.Response;

namespace FlowersApp.Auth.Features.Auth.Commands.GenerateAuthTokens;

public record GenerateAuthTokensCommand(
    AppUser User,
    string Role,
    string? DriverStatus = null
) : ICommand<AuthResponse>;

public class GenerateAuthTokensCommandHandler : ICommandHandler<GenerateAuthTokensCommand, AuthResponse>
{
    private readonly ITokenService _tokenService;

    public GenerateAuthTokensCommandHandler(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    public async Task<RequestResult<AuthResponse>> Handle(GenerateAuthTokensCommand request, CancellationToken cancellationToken)
    {
        var (accessToken, expiresAt) = await _tokenService.GenerateAccessTokenAsync(
            request.User,
            request.Role,
            cancellationToken
        );

        var refreshToken = await _tokenService.GenerateRefreshTokenAsync(
            request.User.Id,
            cancellationToken
        );

        var response = new AuthResponse(
            UserId: request.User.Id,
            Email: request.User.Email ?? string.Empty,
            FullName: request.User.FullName ?? string.Empty,
            Role: request.Role,
            AccessToken: accessToken,
            RefreshToken: refreshToken.Token,
            AccessTokenExpiresAt: expiresAt,
            DriverStatus: request.DriverStatus,
            ProfilePhotoUrl: request.User.ProfilePhotoUrl
        );

        return RequestResult<AuthResponse>.succeeded(response, ResultCode.LoginSuccessful);
    }
}
