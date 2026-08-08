using FlowerApp.Auth.Common;
using FlowerApp.Auth.Common.Enums;
using FlowerApp.Auth.Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FlowerApp.Auth.Features.Logout
{
    public record LogoutCommand
        : IRequest<ApiResponse<LogoutResponse>>;

    public class LogoutCommandHandler
        : IRequestHandler<LogoutCommand, ApiResponse<LogoutResponse>>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LogoutCommandHandler(
            ApplicationDbContext dbContext,
            IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ApiResponse<LogoutResponse>> Handle(
            LogoutCommand request,
            CancellationToken cancellationToken)
        {
            var user = _httpContextAccessor
                .HttpContext?
                .User;

            // =========================
            // Check Authentication
            // =========================

            if (user?.Identity?.IsAuthenticated != true)
            {
                return ApiResponse<LogoutResponse>.Failure(
                    "You are not logged in.",
                    new List<ErrorCode>
                    {
                        ErrorCode.InvalidCredentials
                    },
                    StatusCodes.Status401Unauthorized);
            }

            // =========================
            // Get ID from JWT
            // =========================

            var userIdClaim =
                user.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? user.FindFirst("sub")?.Value;
            
            var role =
                user.FindFirst(ClaimTypes.Role)?.Value;

            if (string.IsNullOrWhiteSpace(userIdClaim))
            {
                return ApiResponse<LogoutResponse>.Failure(
                    "Invalid authentication token.",
                    new List<ErrorCode>
                    {
                        ErrorCode.InvalidCredentials
                    },
                    StatusCodes.Status401Unauthorized);
            }

            var now = DateTime.UtcNow;

            // =====================================================
            // GUEST LOGOUT
            // =====================================================

            if (role == "Guest")
            {
                if (!Guid.TryParse(
                        userIdClaim,
                        out var guestSessionId))
                {
                    return ApiResponse<LogoutResponse>.Failure(
                        "Invalid guest session.",
                        new List<ErrorCode>
                        {
                            ErrorCode.InvalidCredentials
                        },
                        StatusCodes.Status401Unauthorized);
                }

                var guestTokens =
                    await _dbContext.RefreshTokens
                        .Where(t =>
                            t.GuestSessionId == guestSessionId &&
                            !t.RevokedAt.HasValue &&
                            t.ExpiresAt > now)
                        .ToListAsync(cancellationToken);

                if (!guestTokens.Any())
                {
                    return ApiResponse<LogoutResponse>.Success(
                        new LogoutResponse
                        {
                            LoggedOut = true
                        },
                        "Guest already logged out.");
                }

                foreach (var token in guestTokens)
                {
                    token.RevokedAt = now;
                }

                await _dbContext.SaveChangesAsync(
                    cancellationToken);

                return ApiResponse<LogoutResponse>.Success(
                    new LogoutResponse
                    {
                        LoggedOut = true
                    },
                    "Guest logged out successfully.");
            }

            // =====================================================
            // REGISTERED USER LOGOUT
            // =====================================================
            Guid guestSession = Guid.Parse(userIdClaim);
            var userTokens =
                await _dbContext.RefreshTokens
                    .Where(t =>
                        t.GuestSessionId == guestSession &&
                        !t.RevokedAt.HasValue &&
                        t.ExpiresAt > now)
                    .ToListAsync(cancellationToken);

            if (!userTokens.Any())
            {
                return ApiResponse<LogoutResponse>.Success(
                    new LogoutResponse
                    {
                        LoggedOut = true
                    },
                    "Already logged out.");
            }

            foreach (var token in userTokens)
            {
                token.RevokedAt = now;
            }

            await _dbContext.SaveChangesAsync(
                cancellationToken);

            return ApiResponse<LogoutResponse>.Success(
                new LogoutResponse
                {
                    LoggedOut = true
                },
                "User logged out successfully.");
        }
    }
}