using FlowerApp.Auth.Domain.Interfaces;
using FlowerApp.Auth.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FlowerApp.Auth.Infrastructure.Sessions
{
    public class SessionService : ISessionService
    {
        private readonly ApplicationDbContext _db;

        public SessionService(ApplicationDbContext db) => _db = db;

        public async Task RevokeAllSessionsAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var tokens = await _db.RefreshTokens
                .Where(t => t.UserId == userId && t.RevokedAt == null)
                .ToListAsync();

            foreach (var t in tokens)
                t.RevokedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();
        }
    }
}
