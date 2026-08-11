using FlowersApp.Auth.Shared.Interfaces;

namespace FlowersApp.Auth.Features.Login;

public class LoginCommand : ICommand<AuthResponse>
{
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
}
