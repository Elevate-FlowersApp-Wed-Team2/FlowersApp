using FlowersApp.Auth.Shared.Interfaces;
using MediatR;

namespace FlowersApp.Auth.Features.Sessions.RevokeSession;

public record RevokeSessionCommand(Guid SessionId) : ICommand<Unit>;
