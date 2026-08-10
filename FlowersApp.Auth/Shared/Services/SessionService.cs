using FlowerApp.Auth.Domain.Interfaces;
using FlowersApp.Auth.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FlowersApp.Auth.Shared.Services
{
    public class SessionService : ISessionService
    {
        private readonly AppDbContext _db;

        public SessionService(AppDbContext db) => _db = db;

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
