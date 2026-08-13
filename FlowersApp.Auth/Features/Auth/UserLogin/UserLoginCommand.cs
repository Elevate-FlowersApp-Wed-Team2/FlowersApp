using FlowersApp.Auth.Shared.Interfaces;
using FlowersApp.Auth.Shared.Response;

namespace FlowersApp.Auth.Features.Auth.UserLogin;

public record UserLoginCommand(string Email, string Password) : ICommand<AuthResponse>;
