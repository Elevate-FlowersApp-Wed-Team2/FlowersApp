using FlowerApp.Auth.Domain.Interfaces;
using FlowersApp.Auth.Shared.Interfaces;
using FlowersApp.Auth.Shared.Response;
using MediatR;

namespace FlowersApp.Auth.Features.Sessions.RevokeSession;

public class RevokeSessionHandler : ICommandHandler<RevokeSessionCommand, Unit>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly ISessionService _sessionService;

    public RevokeSessionHandler(ICurrentUserService currentUserService, ISessionService sessionService)
    {
        _currentUserService = currentUserService;
        _sessionService = sessionService;
    }

    public async Task<RequestResult<Unit>> Handle(
        RevokeSessionCommand request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(_currentUserService.UserId) ||
            !Guid.TryParse(_currentUserService.UserId, out var userId))
        {
            return RequestResult<Unit>.Failure(ResultCode.UserNotFound);
        }

        var sessions = await _sessionService.GetActiveSessionsAsync(userId, cancellationToken);
        if (sessions.All(s => s.Id != request.SessionId))
            return RequestResult<Unit>.Failure(ResultCode.SessionNotFound);

        await _sessionService.RevokeSessionAsync(userId, request.SessionId, cancellationToken);
        return RequestResult<Unit>.succeeded(Unit.Value, ResultCode.SessionRevokedSuccessfully);
    }
}
