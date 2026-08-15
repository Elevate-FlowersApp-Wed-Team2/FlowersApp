using FlowersApp.Auth.Shared.Interfaces;
using FlowersApp.Shared.Redis;

namespace FlowersApp.Auth.Shared.Services;

public class RedisLoginRateLimiter : ILoginRateLimiter
{
    private const int MaxAttempts = 5;
    private static readonly TimeSpan LockoutWindow = TimeSpan.FromMinutes(15);

    private readonly IRedisCacheService _redis;

    public RedisLoginRateLimiter(IRedisCacheService redis)
    {
        _redis = redis;
    }

    public async Task<bool> IsRateLimitedAsync(
        string email,
        string? ipAddress,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        try
        {
            // Primary lockout is per-account so IP rotation cannot bypass it.
            if (await ExceedsAsync(EmailKey(email)))
                return true;

            // Optional secondary throttle per IP (shared Wi-Fi friendly threshold).
            if (!string.IsNullOrWhiteSpace(ipAddress) && await ExceedsAsync(IpKey(ipAddress)))
                return true;

            return false;
        }
        catch
        {
            return false;
        }
    }

    public async Task RecordFailedAttemptAsync(
        string email,
        string? ipAddress,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(email))
            return;

        try
        {
            await IncrementWithExpiryAsync(EmailKey(email));
            if (!string.IsNullOrWhiteSpace(ipAddress))
                await IncrementWithExpiryAsync(IpKey(ipAddress));
        }
        catch
        {
            // Ignore Redis connectivity issues so login remains available.
        }
    }

    public async Task ResetAttemptsAsync(
        string email,
        string? ipAddress,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(email))
            return;

        try
        {
            await _redis.DeleteAsync(EmailKey(email));
            // Do not clear the IP counter on success — it protects other accounts on the same IP.
        }
        catch
        {
            // Ignore Redis connectivity issues.
        }
    }

    private async Task<bool> ExceedsAsync(string key)
    {
        var attemptsStr = await _redis.GetAsync(key);
        return attemptsStr is not null
               && int.TryParse(attemptsStr, out var attempts)
               && attempts >= MaxAttempts;
    }

    private async Task IncrementWithExpiryAsync(string key)
    {
        var count = await _redis.IncrementAsync(key);
        if (count == 1)
            await _redis.ExpireAsync(key, LockoutWindow);
    }

    private static string EmailKey(string email) =>
        $"login_attempts:email:{email.Trim().ToLowerInvariant()}";

    private static string IpKey(string ipAddress) =>
        $"login_attempts:ip:{ipAddress.Trim()}";
}
