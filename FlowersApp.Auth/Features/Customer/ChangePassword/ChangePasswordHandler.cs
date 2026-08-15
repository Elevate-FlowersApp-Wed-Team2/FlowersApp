using FlowerApp.Auth.Domain;
using FlowerApp.Auth.Domain.Interfaces;
using FlowersApp.Auth.Domain.Entities;
using FlowersApp.Auth.Features.Customer.ChangePassword.Events;
using FlowersApp.Auth.Shared.Interfaces;
using FlowersApp.Auth.Shared.Response;
using FlowersApp.Auth.Shared.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Drawing;

namespace FlowersApp.Auth.Features.Customer.ChangePassword
{
    // Message is deliberately never set here - LocalizationBehavior fills
    // response.Message from response.Code (via ResultCode.Localize(localizer))
    // after this handler returns. The handler only ever picks a ResultCode.
    public class ChangePasswordCommandHandler : ICommandHandler<ChangePasswordCommand, Unit>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ICurrentUserService _currentUserService;
        private readonly ISessionService _sessionService;
        private readonly IPublisher _publisher;
        private readonly ILogger<ChangePasswordCommandHandler> _logger;

        public ChangePasswordCommandHandler(
            UserManager<AppUser> userManager,
            ICurrentUserService currentUserService,
            ISessionService sessionService,
            IPublisher publisher,
            ILogger<ChangePasswordCommandHandler> logger)
        {
            _userManager = userManager;
            _currentUserService = currentUserService;
            _sessionService = sessionService;
            _publisher = publisher;
            _logger = logger;
        }

        public async Task<RequestResult<Unit>> Handle(
            ChangePasswordCommand request,
            CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;
            _logger.LogInformation("Attempting password change for user {UserId}.", userId);

            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user is null)
            {
                _logger.LogWarning("Password change failed: User {UserId} not found.", userId);
                return RequestResult<Unit>.Failure(ResultCode.UserNotFound);
            }

            // ChangePasswordAsync validates the current password internally -
            // never call CheckPasswordAsync beforehand, it causes double hashing.
            var identityResult = await _userManager.ChangePasswordAsync(
                user, request.CurrentPassword, request.NewPassword);

            if (!identityResult.Succeeded)
            {
                var isCurrentPasswordWrong = identityResult.Errors
                    .Any(e => e.Code == "PasswordMismatch");

                var code = isCurrentPasswordWrong
                    ? ResultCode.CurrentPasswordIncorrect
                    : ResultCode.PasswordChangeFailed;

                if (isCurrentPasswordWrong)
                {
                    _logger.LogWarning("Password change failed for user {UserId}: Current password incorrect.", userId);
                }
                else
                {
                    _logger.LogError("Identity error(s) during password change for user {UserId}: {Errors}",
                        userId, string.Join(", ", identityResult.Errors.Select(e => e.Description)));
                }

                return RequestResult<Unit>.Failure(code);
            }
            await _sessionService.RevokeAllSessionsAsync(user.Id, cancellationToken);
            await _publisher.Publish(new PasswordChangedEvent(user.Id, user.Email!), cancellationToken);
            _logger.LogInformation("Password changed successfully for user {UserId}.", userId);
            return RequestResult<Unit>.succeeded(Unit.Value, ResultCode.PasswordChangedSuccessfully);
        }
    }
}



