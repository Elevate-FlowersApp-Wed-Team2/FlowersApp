namespace FlowersApp.Auth.Shared.Response;

public record AuthResponse(
    Guid UserId,
    string Email,
    string FullName,
    string Role,
    string AccessToken,
    string RefreshToken,
    DateTime AccessTokenExpiresAt,
    string? DriverStatus = null,
    string? ProfilePhotoUrl = null
);
