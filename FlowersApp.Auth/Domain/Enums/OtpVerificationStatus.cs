namespace FlowersApp.Auth.Domain.Enums
{
    public enum OtpVerificationStatus
    {
        Success,
        Invalid,
        Expired,
        MaxAttemptsExceeded
    }
}
