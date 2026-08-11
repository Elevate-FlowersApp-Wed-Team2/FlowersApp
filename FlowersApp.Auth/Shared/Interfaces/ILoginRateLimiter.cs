namespace FlowersApp.Auth.Shared.Interfaces;

public interface ILoginRateLimiter
{
    Task<bool> IsRateLimitedAsync(string email, CancellationToken cancellationToken = default);
    Task RecordFailedAttemptAsync(string email, CancellationToken cancellationToken = default);
    Task ResetAttemptsAsync(string email, CancellationToken cancellationToken = default);
}
