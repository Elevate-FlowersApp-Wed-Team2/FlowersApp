namespace FlowerApp.Auth.Domain.Interfaces
{
    public interface ISessionService
    {
        Task RevokeAllSessionsAsync(Guid userId, CancellationToken cancellationToken = default);
    }
}
