namespace FlowerApp.Auth.Features.Login
{
    public record LoginResponse(
        string AccessToken,
        string RefreshToken,
        int ExpiresIn,
        string Role,
        string? ApplicationStatus
        );
    
}
