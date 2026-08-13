using FlowersApp.Auth.Domain.Enums;
using FlowersApp.Auth.Shared;
using FlowersApp.Auth.Shared.Interfaces;
using FlowersApp.Shared.Redis;
using System.Security.Cryptography;

namespace FlowersApp.Auth.Infrastructure.Services
{
    public class PasswordResetOtpService : IPasswordResetOtpService
    {
        private const string OtpPrefix = "pwd-reset:otp:";
        private const string AttemptsPrefix = "pwd-reset:attempts:";
        private const string CooldownPrefix = "pwd-reset:cooldown:";
        private const string TokenPrefix = "pwd-reset:token:";

        private static readonly TimeSpan OtpTtl = TimeSpan.FromMinutes(10);
        private static readonly TimeSpan CooldownTtl = TimeSpan.FromSeconds(30);
        private static readonly TimeSpan ResetTokenTtl = TimeSpan.FromMinutes(10);
        private const int MaxAttempts = 5;

        private readonly IRedisCacheService _cache;

        public PasswordResetOtpService(IRedisCacheService cache)
        {
            _cache = cache;
        }

        public async Task<string> GenerateOtpAsync(string email)
        {
            var key = Normalize(email);
            var otp = RandomNumberGenerator.GetInt32(0, 1_000_000).ToString("D6");

            await _cache.SetAsync(OtpKey(key), otp, OtpTtl);
            await _cache.DeleteAsync(AttemptsKey(key)); // fresh attempts count for the new code
            await _cache.SetAsync(CooldownKey(key), "1", CooldownTtl);

            return otp;
        }

        public Task<bool> IsResendOnCooldownAsync(string email)
            => _cache.ExistsAsync(CooldownKey(Normalize(email)));

        public async Task<OtpValidationOutcome> ValidateOtpAsync(string email, string otp)
        {
            var key = Normalize(email);
            var storedOtp = await _cache.GetAsync(OtpKey(key));

            if (storedOtp is null)
                return new OtpValidationOutcome { Result = OtpValidation.Expired, AttemptsRemaining = 0 };

            if (!string.Equals(storedOtp, otp, StringComparison.Ordinal))
            {
                var attempts = await _cache.IncrementAsync(AttemptsKey(key));
                await _cache.ExpireAsync(AttemptsKey(key), OtpTtl); // don't let the counter outlive the OTP

                if (attempts >= MaxAttempts)
                {
                    await _cache.DeleteAsync(OtpKey(key));
                    await _cache.DeleteAsync(AttemptsKey(key));
                    return new OtpValidationOutcome { Result = OtpValidation.MaxAttemptsExceeded, AttemptsRemaining = 0 };
                }

                return new OtpValidationOutcome
                {
                    Result = OtpValidation.InvalidCode,
                    AttemptsRemaining = (int)(MaxAttempts - attempts)
                };
            }

            await _cache.DeleteAsync(OtpKey(key));
            await _cache.DeleteAsync(AttemptsKey(key));
            return new OtpValidationOutcome { Result = OtpValidation.Valid, AttemptsRemaining = MaxAttempts };
        }

        public async Task<string> IssueResetTokenAsync(string email)
        {
            var key = Normalize(email);
            var token = Guid.NewGuid().ToString("N");
            await _cache.SetAsync(TokenKey(key), token, ResetTokenTtl);
            return token;
        }

        public async Task<bool> ValidateResetTokenAsync(string email, string token)
        {
            var storedToken = await _cache.GetAsync(TokenKey(Normalize(email)));
            return storedToken is not null && string.Equals(storedToken, token, StringComparison.Ordinal);
        }

        public Task InvalidateResetTokenAsync(string email)
            => _cache.DeleteAsync(TokenKey(Normalize(email)));

        private static string Normalize(string email) => email.Trim().ToLowerInvariant();
        private static string OtpKey(string email) => $"{OtpPrefix}{email}";
        private static string AttemptsKey(string email) => $"{AttemptsPrefix}{email}";
        private static string CooldownKey(string email) => $"{CooldownPrefix}{email}";
        private static string TokenKey(string email) => $"{TokenPrefix}{email}";
    }
}
