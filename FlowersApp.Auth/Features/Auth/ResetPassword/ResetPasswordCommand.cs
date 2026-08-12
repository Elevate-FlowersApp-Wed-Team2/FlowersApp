using FlowersApp.Auth.Shared.Interfaces;

namespace FlowersApp.Auth.Features.Auth.ResetPassword;

public record ResetPasswordCommand(
    string ResetToken,
    string NewPassword,
    string ConfirmNewPassword) : ICommand<object?>;
