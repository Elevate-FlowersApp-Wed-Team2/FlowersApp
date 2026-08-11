using FlowersApp.Auth.Shared.Interfaces;
using FlowersApp.Shared.Redis;

namespace FlowersApp.Auth.Shared.Services;

public class RedisLoginRateLimiter : ILoginRateLimiter
{
    private readonly IRedisCacheService _redisCacheService;
    private const int MaxAttempts = 5;
    private static readonly TimeSpan LockoutWindow = TimeSpan.FromMinutes(15);

    public RedisLoginRateLimiter(IRedisCacheService redisCacheService)
    {
        _redisCacheService = redisCacheService;
    }

    public async Task<bool> IsRateLimitedAsync(string email, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(email)) return false;

        try
        {
            var key = GetKey(email);
            var attemptsStr = await _redisCacheService.GetAsync(key);
            if (attemptsStr is not null && int.TryParse(attemptsStr, out var attempts))
            {
                return attempts >= MaxAttempts;
            }
        }
        catch
        {
            // If Redis fails, gracefully allow request rather than crashing infrastructure
        }

        return false;
    }

    public async Task RecordFailedAttemptAsync(string email, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(email)) return;

        try
        {
            var key = GetKey(email);
            var count = await _redisCacheService.IncrementAsync(key);
            if (count == 1)
            {
                await _redisCacheService.ExpireAsync(key, LockoutWindow);
            }
        }
        catch
        {
            // Ignore Redis connectivity issues
        }
    }

    public async Task ResetAttemptsAsync(string email, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(email)) return;

        try
        {
            var key = GetKey(email);
            await _redisCacheService.DeleteAsync(key);
        }
        catch
        {
            // Ignore Redis connectivity issues
        }
    }

    private static string GetKey(string email) => $"login_attempts:{email.Trim().ToLowerInvariant()}";
}
