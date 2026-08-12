namespace FlowersApp.Auth.Shared.Interfaces;

public interface ILoginRateLimiter
{
    Task<bool> IsRateLimitedAsync(string email, string? ipAddress, CancellationToken cancellationToken = default);
    Task RecordFailedAttemptAsync(string email, string? ipAddress, CancellationToken cancellationToken = default);
    Task ResetAttemptsAsync(string email, string? ipAddress, CancellationToken cancellationToken = default);
}
