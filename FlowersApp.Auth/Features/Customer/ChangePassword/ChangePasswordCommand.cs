using FlowersApp.Auth.Shared.Interfaces;
using MediatR;

namespace FlowersApp.Auth.Features.Customer.ChangePassword
{
    public class ChangePasswordCommand : ICommand<Unit>
    {
        public string CurrentPassword { get; set; } = default!;
        public string NewPassword { get; set; } = default!;
        public string ConfirmNewPassword { get; set; } = default!;
    }
}
