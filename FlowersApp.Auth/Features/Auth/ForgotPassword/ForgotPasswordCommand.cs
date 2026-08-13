using FlowersApp.Auth.Shared.Interfaces;

namespace FlowersApp.Auth.Features.Auth.ForgotPassword;

public record ForgotPasswordCommand(string Email) : ICommand<object?>;
