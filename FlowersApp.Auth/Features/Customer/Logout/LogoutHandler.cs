using FlowerApp.Auth.Domain.Interfaces;
using FlowersApp.Auth.Domain.Entities;
using FlowersApp.Auth.Shared.Interfaces;
using FlowersApp.Auth.Shared.Response;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FlowersApp.Auth.Features.Customer.Logout
{
    public class LogoutCommandHandler : ICommandHandler<LogoutCommand, Unit>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ICurrentUserService _currentUserService;
        private readonly ISessionService _sessionService;
        private readonly ILogger<LogoutCommandHandler> _logger;

        public LogoutCommandHandler(
            UserManager<AppUser> userManager,
            ICurrentUserService currentUserService,
            ISessionService sessionService,
            ILogger<LogoutCommandHandler> logger)
        {
            _userManager = userManager;
            _currentUserService = currentUserService;
            _sessionService = sessionService;
            _logger = logger;
        }

        public async Task<RequestResult<Unit>> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;
            _logger.LogInformation("Attempting logout for user {UserId}.", userId);

            if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var userGuid))
            {
                _logger.LogWarning("Logout failed: UserId from current user service is null or empty.");
                return RequestResult<Unit>.Failure(ResultCode.UserNotFound);
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
            {
                _logger.LogWarning("Logout failed: User {UserId} not found in database.", userId);
                return RequestResult<Unit>.Failure(ResultCode.UserNotFound);
            }

            if (!string.IsNullOrWhiteSpace(request.RefreshToken))
            {
                var session = await _sessionService.FindByRawTokenAsync(request.RefreshToken, cancellationToken);
                if (session is not null && session.UserId == userGuid && session.RevokedAt is null)
                {
                    await _sessionService.RevokeSessionAsync(userGuid, session.Id, cancellationToken);
                    _logger.LogInformation(
                        "User {UserId} logged out current session {SessionId}.",
                        userId,
                        session.Id);
                }
                else
                {
                    // Token unknown/already revoked — still succeed; client should clear local state.
                    _logger.LogInformation(
                        "Logout for user {UserId}: refresh token not found or already revoked.",
                        userId);
                }
            }
            else
            {
                await _sessionService.RevokeAllSessionsAsync(user.Id, cancellationToken);
                _logger.LogInformation("User {UserId} logged out successfully. Revoked all sessions.", userId);
            }

            return RequestResult<Unit>.succeeded(Unit.Value, ResultCode.LoggedOutSuccessfully);
        }
    }
}
