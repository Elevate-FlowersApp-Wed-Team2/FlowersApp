using FlowersApp.Auth.Domain.Entities;

namespace FlowerApp.Auth.Domain.Interfaces
{
    public interface ISessionService
    {
        Task RevokeAllSessionsAsync(Guid userId, CancellationToken cancellationToken = default);
        Task RevokeSessionAsync(Guid userId, Guid sessionId, CancellationToken cancellationToken = default);
        Task RevokeTokenFamilyAsync(Guid familyId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<RefreshToken>> GetActiveSessionsAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<RefreshToken?> FindByRawTokenAsync(string rawToken, CancellationToken cancellationToken = default);
        Task MarkReplacedAsync(RefreshToken oldToken, Guid newTokenId, CancellationToken cancellationToken = default);
    }
}
