namespace FlowersApp.Auth.Features.Login;

public class AuthResponse
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public int ExpiresIn { get; set; }
    public string Role { get; set; } = string.Empty;
    public string? DriverStatus { get; set; }
}
