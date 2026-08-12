using FlowersApp.Auth.Domain.Entities;

namespace FlowersApp.Auth.Infrastructure.Services;

public record TokenResult(string AccessToken, string RefreshToken, int ExpiresIn);

public interface ITokenService
{
    TokenResult GenerateTokens(AppUser user, IEnumerable<string> roles, string? driverStatus = null);
}
