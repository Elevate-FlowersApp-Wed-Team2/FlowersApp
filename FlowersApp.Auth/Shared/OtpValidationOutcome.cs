using FlowersApp.Auth.Domain.Enums;

namespace FlowersApp.Auth.Shared
{
    public class OtpValidationOutcome
    {
        public OtpValidation Result { get; init; }
        public int AttemptsRemaining { get; init; }
    }
}
