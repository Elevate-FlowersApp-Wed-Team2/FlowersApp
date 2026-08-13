using FlowersApp.Auth.Shared.Interfaces;

namespace FlowersApp.Auth.Features.Account.ResetPassword
{
    public record ResetPasswordOrchestrator(
    string Email, string ResetToken, string NewPassword, string ConfirmNewPassword
    ) : ICommand<ResetPasswordResponse>;
    public record ResetPasswordResponse(string Message);
}
