namespace FlowersApp.Auth.Shared.Interfaces;

public interface IPasswordResetOtpService
{
    /// <summary>
    /// Email-keyed resend cooldown that works the same for known and unknown accounts.
    /// </summary>
    Task<(bool Allowed, TimeSpan? RetryAfter)> TryBeginIssueByEmailAsync(
        string email,
        CancellationToken cancellationToken = default);

    Task StoreOtpAsync(Guid userId, string otp, CancellationToken cancellationToken = default);

    Task<(OtpVerificationStatus Status, string? ResetToken)> VerifyOtpAsync(
        Guid userId,
        string otp,
        CancellationToken cancellationToken = default);

    Task<Guid?> ConsumeResetTokenAsync(string resetToken, CancellationToken cancellationToken = default);
}

public enum OtpVerificationStatus
{
    Success,
    Invalid,
    Expired,
    MaxAttemptsExceeded
}
