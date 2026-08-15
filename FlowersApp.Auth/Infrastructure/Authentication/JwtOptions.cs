namespace FlowersApp.Auth.Infrastructure.Authentication;

public class JwtOptions
{
    public const string SectionName = "Jwt";

    public string Key { get; set; } = string.Empty;
    public string Issuer { get; set; } = "FlowersApp";
    public string Audience { get; set; } = "FlowersAppClient";
    public int DurationInMinutes { get; set; } = 15;
    public int RefreshTokenExpirationDays { get; set; } = 30;
}
