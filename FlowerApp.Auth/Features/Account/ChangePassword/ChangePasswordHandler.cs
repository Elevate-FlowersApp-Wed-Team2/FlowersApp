using FlowerApp.Auth.Domain;
using FlowerApp.Auth.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Shared;

namespace FlowerApp.Auth.Features.Account.ChangePassword
{
    public class ChangePasswordHandler : IRequestHandler<ChangePasswordCommand, Result>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly ISessionService _sessionService;
        private readonly ILogger<ChangePasswordHandler> _logger;

        public ChangePasswordHandler(
            UserManager<ApplicationUser> userManager,
            IEmailSender emailSender,
            ISessionService sessionService,
            ILogger<ChangePasswordHandler> logger)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _sessionService = sessionService;
            _logger = logger;
        }

        public async Task<Result> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());
            if (user is null)
                return Result.Failure("User not found.", ErrorType.NotFound);

            var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);

            if (!result.Succeeded)
            {
                var isWrongCurrentPassword = result.Errors.Any(e => e.Code == "PasswordMismatch");

                return isWrongCurrentPassword
                       ? Result.Failure("Current password is incorrect.", ErrorType.Validation)
                       : Result.Failure(string.Join("\n", result.Errors.Select(e => e.Description)));
            }
            // AC6 — revoke sessions, notify
            await _sessionService.RevokeAllSessionsAsync(user.Id);
            
            try
            {
                await _emailSender.SendAsync(user.Email, "Your password was changed", "Your account password was just changed. If this wasn't you, contact support immediately", cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send password change notification email to {Email}", user.Email);

            }
            return Result.Success();
        }
    }
}
