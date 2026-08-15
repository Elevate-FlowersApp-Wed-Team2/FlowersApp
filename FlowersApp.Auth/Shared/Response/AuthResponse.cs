namespace FlowersApp.Auth.Shared.Response;

public record AuthResponse(
    string AccessToken,
    string RefreshToken,
    int ExpiresIn,
    string Role,
    string? DriverStatus = null);
