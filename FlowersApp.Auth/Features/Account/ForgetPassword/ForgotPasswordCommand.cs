using FlowersApp.Auth.Shared.Interfaces;
using MediatR;
using Superpower.Model;

namespace FlowersApp.Auth.Features.Account.ForgetPassword
{
    public record ForgotPasswordCommand(string Email) : ICommand<ForgotPasswordResponse>;
    public record ForgotPasswordResponse(string Message);
}
