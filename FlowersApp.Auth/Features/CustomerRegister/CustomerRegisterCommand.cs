using FlowersApp.Auth.Domain.Enums;
using FlowersApp.Auth.Shared.Interfaces;

namespace FlowersApp.Auth.Features.CustomerRegister;

public class CustomerRegisterCommand : ICommand<Guid>
{
    public string FullName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
    public Gender Gender { get; set; }
    public string Password { get; set; } = default!;
    public string ConfirmPassword { get; set; } = default!;
}
