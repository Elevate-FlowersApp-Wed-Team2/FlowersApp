namespace FlowersApp.Auth.Domain.Enums
{
    public enum OtpValidation
    {
        Valid,
        InvalidCode,
        Expired,
        MaxAttemptsExceeded
    }
}
