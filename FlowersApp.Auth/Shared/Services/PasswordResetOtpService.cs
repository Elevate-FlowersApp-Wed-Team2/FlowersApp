using FlowersApp.Auth.Shared.Interfaces;
using FlowersApp.Shared.Redis;
using System.Security.Cryptography;
using System.Text;

namespace FlowersApp.Auth.Shared.Services;

public class PasswordResetOtpService : IPasswordResetOtpService
{
    private static readonly TimeSpan OtpLifetime = TimeSpan.FromMinutes(10);
    private static readonly TimeSpan ResendCooldown = TimeSpan.FromSeconds(30);
    private static readonly TimeSpan ResetTokenLifetime = TimeSpan.FromMinutes(10);
    private const int MaxAttempts = 5;

    private readonly IRedisCacheService _redis;

    public PasswordResetOtpService(IRedisCacheService redis)
    {
        _redis = redis;
    }

    public async Task<(bool Allowed, TimeSpan? RetryAfter)> TryBeginIssueByEmailAsync(
        string email,
        CancellationToken cancellationToken = default)
    {
        var resendKey = ResendEmailKey(email);
        if (await _redis.ExistsAsync(resendKey))
            return (false, ResendCooldown);

        await _redis.SetAsync(resendKey, "1", ResendCooldown);
        return (true, null);
    }

    public async Task StoreOtpAsync(Guid userId, string otp, CancellationToken cancellationToken = default)
    {
        await _redis.SetAsync(OtpKey(userId), Hash(otp), OtpLifetime);
        await _redis.DeleteAsync(AttemptsKey(userId));
    }

    public async Task<(OtpVerificationStatus Status, string? ResetToken)> VerifyOtpAsync(
        Guid userId,
        string otp,
        CancellationToken cancellationToken = default)
    {
        var otpKey = OtpKey(userId);
        var storedHash = await _redis.GetAsync(otpKey);
        if (storedHash is null)
            return (OtpVerificationStatus.Expired, null);

        var attemptsKey = AttemptsKey(userId);
        var attemptsRaw = await _redis.GetAsync(attemptsKey);
        var attempts = int.TryParse(attemptsRaw, out var parsed) ? parsed : 0;
        if (attempts >= MaxAttempts)
        {
            await _redis.DeleteAsync(otpKey);
            await _redis.DeleteAsync(attemptsKey);
            return (OtpVerificationStatus.MaxAttemptsExceeded, null);
        }

        var providedHash = Hash(otp);
        var matches = storedHash.Length == providedHash.Length &&
                      CryptographicOperations.FixedTimeEquals(
                          Encoding.UTF8.GetBytes(storedHash),
                          Encoding.UTF8.GetBytes(providedHash));

        if (!matches)
        {
            var next = await _redis.IncrementAsync(attemptsKey);
            if (next == 1)
                await _redis.ExpireAsync(attemptsKey, OtpLifetime);

            if (next >= MaxAttempts)
            {
                await _redis.DeleteAsync(otpKey);
                await _redis.DeleteAsync(attemptsKey);
                return (OtpVerificationStatus.MaxAttemptsExceeded, null);
            }

            return (OtpVerificationStatus.Invalid, null);
        }

        await _redis.DeleteAsync(otpKey);
        await _redis.DeleteAsync(attemptsKey);

        var resetToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
        await _redis.SetAsync(ResetTokenKey(resetToken), userId.ToString(), ResetTokenLifetime);
        return (OtpVerificationStatus.Success, resetToken);
    }

    public async Task<Guid?> ConsumeResetTokenAsync(string resetToken, CancellationToken cancellationToken = default)
    {
        var key = ResetTokenKey(resetToken);
        var userIdRaw = await _redis.GetAsync(key);
        if (userIdRaw is null || !Guid.TryParse(userIdRaw, out var userId))
            return null;

        await _redis.DeleteAsync(key);
        return userId;
    }

    private static string OtpKey(Guid userId) => $"pwdreset:otp:{userId:D}";
    private static string AttemptsKey(Guid userId) => $"pwdreset:attempts:{userId:D}";
    private static string ResendEmailKey(string email) =>
        $"pwdreset:resend:email:{email.Trim().ToLowerInvariant()}";
    private static string ResetTokenKey(string token) => $"pwdreset:token:{token}";

    private static string Hash(string value)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(value));
        return Convert.ToBase64String(bytes);
    }
}
