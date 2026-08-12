using FlowersApp.Auth.Shared.Interfaces;
using FlowersApp.Auth.Shared.Response;

namespace FlowersApp.Auth.Features.Auth.RefreshToken;

public record RefreshTokenCommand(string RefreshToken, string? DeviceInfo = null) : ICommand<AuthResponse>;
