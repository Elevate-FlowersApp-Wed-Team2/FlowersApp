using FlowersApp.Auth.Shared.Interfaces;
using FlowersApp.Auth.Shared.Response;

namespace FlowersApp.Auth.Features.Auth.DriverLogin;

public record DriverLoginOrchestrator(
    string Email,
    string Password
) : ICommand<AuthResponse>;
