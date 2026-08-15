using FlowerApp.Auth.Domain.Interfaces;
using FlowersApp.Auth.Domain.Entities;
using FlowersApp.Auth.Infrastructure.Persistence;
using FlowersApp.Auth.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FlowersApp.Auth.Shared.Services
{
    public class SessionService : ISessionService
    {
        private readonly AppDbContext _db;
        private readonly ITokenService _tokenService;

        public SessionService(AppDbContext db, ITokenService tokenService)
        {
            _db = db;
            _tokenService = tokenService;
        }

        public async Task RevokeAllSessionsAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var tokens = await _db.RefreshTokens
                .AsTracking()
                .Where(t => t.UserId == userId && t.RevokedAt == null)
                .ToListAsync(cancellationToken);

            var now = DateTime.UtcNow;
            foreach (var t in tokens)
                t.RevokedAt = now;

            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task RevokeSessionAsync(Guid userId, Guid sessionId, CancellationToken cancellationToken = default)
        {
            var token = await _db.RefreshTokens
                .AsTracking()
                .FirstOrDefaultAsync(t => t.Id == sessionId && t.UserId == userId, cancellationToken);

            if (token is null || token.RevokedAt is not null)
                return;

            token.RevokedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task RevokeTokenFamilyAsync(Guid familyId, CancellationToken cancellationToken = default)
        {
            var tokens = await _db.RefreshTokens
                .AsTracking()
                .Where(t => t.FamilyId == familyId && t.RevokedAt == null)
                .ToListAsync(cancellationToken);

            var now = DateTime.UtcNow;
            foreach (var t in tokens)
                t.RevokedAt = now;

            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<RefreshToken>> GetActiveSessionsAsync(
            Guid userId,
            CancellationToken cancellationToken = default)
        {
            var now = DateTime.UtcNow;
            return await _db.RefreshTokens
                .AsNoTracking()
                .Where(t => t.UserId == userId && t.RevokedAt == null && t.ExpiresAt > now)
                .OrderByDescending(t => t.IssuedAt)
                .ToListAsync(cancellationToken);
        }

        public async Task<RefreshToken?> FindByRawTokenAsync(
            string rawToken,
            CancellationToken cancellationToken = default)
        {
            var hash = _tokenService.HashToken(rawToken);
            return await _db.RefreshTokens
                .AsTracking()
                .FirstOrDefaultAsync(t => t.Token == hash, cancellationToken);
        }

        public async Task MarkReplacedAsync(
            RefreshToken oldToken,
            Guid newTokenId,
            CancellationToken cancellationToken = default)
        {
            oldToken.RevokedAt = DateTime.UtcNow;
            oldToken.ReplacedByTokenId = newTokenId;
            await _db.SaveChangesAsync(cancellationToken);
        }
    }
}
