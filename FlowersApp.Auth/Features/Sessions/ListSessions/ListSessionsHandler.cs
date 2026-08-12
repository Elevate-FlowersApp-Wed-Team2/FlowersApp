using FlowerApp.Auth.Domain.Interfaces;
using FlowersApp.Auth.Shared.Interfaces;
using FlowersApp.Auth.Shared.Response;

namespace FlowersApp.Auth.Features.Sessions.ListSessions;

public class ListSessionsHandler : IQueryHandler<ListSessionsQuery, IReadOnlyList<SessionDto>>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly ISessionService _sessionService;

    public ListSessionsHandler(ICurrentUserService currentUserService, ISessionService sessionService)
    {
        _currentUserService = currentUserService;
        _sessionService = sessionService;
    }

    public async Task<RequestResult<IReadOnlyList<SessionDto>>> Handle(
        ListSessionsQuery request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(_currentUserService.UserId) ||
            !Guid.TryParse(_currentUserService.UserId, out var userId))
        {
            return RequestResult<IReadOnlyList<SessionDto>>.Failure(ResultCode.UserNotFound);
        }

        var sessions = await _sessionService.GetActiveSessionsAsync(userId, cancellationToken);
        var dto = sessions
            .Select(s => new SessionDto(s.Id, s.DeviceInfo, s.IpAddress, s.IssuedAt, s.ExpiresAt))
            .ToList();

        return RequestResult<IReadOnlyList<SessionDto>>.succeeded(dto, ResultCode.SessionsRetrievedSuccessfully);
    }
}
