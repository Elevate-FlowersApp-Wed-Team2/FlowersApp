using FlowersApp.Auth.Shared.Interfaces;

namespace FlowersApp.Auth.Features.Account.VerifyResetOtp
{
    public record VerifyResetOtpCommand(string Email, string Otp) 
        : ICommand<VerifyResetOtpResponse>;

    public record VerifyResetOtpResponse(string ResetToken, int ExpiresInSeconds,
        int AttemptsRemaining);

}
