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

        public ChangePasswordHandler(
            UserManager<ApplicationUser> userManager,
            IEmailSender emailSender,
            ISessionService sessionService)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _sessionService = sessionService;
        }

        public async Task<Result> Handle(ChangePasswordCommand request, CancellationToken ct)
        {
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());
            if (user is null)
                return Result.Failure("Current password is incorrect", 400); 

            // verify current password FIRST, 
            var currentPasswordValid = await _userManager.CheckPasswordAsync(user, request.CurrentPassword);
            if (!currentPasswordValid)
                return Result.Failure("Current password is incorrect", 400);

            var sameAsCurrent = await _userManager.CheckPasswordAsync(user, request.NewPassword);
            if (sameAsCurrent)
                return Result.Failure("New password must differ from current password", 400);

            // password policy (length/uppercase/digit) to the new one, then hashes + saves.
            var identityResult = await _userManager.ChangePasswordAsync(
                user, request.CurrentPassword, request.NewPassword);

            if (!identityResult.Succeeded)
            {
                var message = string.Join("; ", identityResult.Errors.Select(e => e.Description));
                return Result.Failure(message, 400);
            }

            // AC6 — revoke sessions, notify
            await _sessionService.RevokeAllSessionsAsync(user.Id);

            await _emailSender.SendAsync(
                user.Email!,
                "Your password was changed",
                "Your account password was just changed. If this wasn't you, contact support immediately.");

            return Result.Success();
        }
    }
}
