namespace FlowersApp.Auth.Shared.Interfaces
{
    public interface IPasswordResetOtpService
    {
        /// Generates a new 6-digit OTP, stores it, resets attempts, and starts the resend cooldown. Returns the OTP for the caller to email.</summary>
        Task<string> GenerateOtpAsync(string email);

        Task<bool> IsResendOnCooldownAsync(string email);

        /// <summary>Validates the OTP. On success, clears the OTP + attempts. On wrong code, increments attempts and may invalidate after too many tries.</summary>
        Task<OtpValidationOutcome> ValidateOtpAsync(string email, string otp);

        /// <summary>Issues a short-lived, single-use token authorizing the actual password reset.</summary>
        Task<string> IssueResetTokenAsync(string email);

        Task<bool> ValidateResetTokenAsync(string email, string token);

        Task InvalidateResetTokenAsync(string email);
    }
}
