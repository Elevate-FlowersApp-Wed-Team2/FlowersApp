using FlowerApp.Auth.Domain.Interfaces;
using FlowersApp.Auth.Domain.Entities;
using FlowersApp.Auth.Shared.Interfaces;
using FlowersApp.Auth.Shared.Response;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

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

            if (string.IsNullOrEmpty(userId))
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

            await _sessionService.RevokeAllSessionsAsync(user.Id, cancellationToken);
            _logger.LogInformation("User {UserId} logged out successfully. Revoked all sessions.", userId);

            return RequestResult<Unit>.succeeded(Unit.Value, ResultCode.LoggedOutSuccessfully);
        }
    }
}
