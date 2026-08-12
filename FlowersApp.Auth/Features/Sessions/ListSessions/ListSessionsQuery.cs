using FlowersApp.Auth.Shared.Interfaces;

namespace FlowersApp.Auth.Features.Sessions.ListSessions;

public record ListSessionsQuery : IQuery<IReadOnlyList<SessionDto>>;

public record SessionDto(
    Guid Id,
    string? DeviceInfo,
    string? IpAddress,
    DateTime IssuedAt,
    DateTime ExpiresAt);
