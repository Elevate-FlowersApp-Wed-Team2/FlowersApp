using FlowerApp.Auth.Domain;
using FlowerApp.Auth.Domain.Interfaces;
using FlowerApp.Auth.Features.Account.ChangePassword;
using FlowerApp.Auth.Features.ChangePassword;
using FlowersApp.Auth.Domain.Entities;
using FlowersApp.Auth.Shared.Interfaces;
using FlowersApp.Auth.Shared.Response;
using FlowersApp.Auth.Shared.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Drawing;

namespace FlowerApp.Auth.Features.ChangePassword
{
    // Message is deliberately never set here - LocalizationBehavior fills
    // response.Message from response.Code (via ResultCode.Localize(localizer))
    // after this handler returns. The handler only ever picks a ResultCode.
    public class ChangePasswordCommandHandler : ICommandHandler<ChangePasswordCommand, Unit>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ICurrentUserService _currentUserService;
        private readonly ISessionService _sessionService;

        public ChangePasswordCommandHandler(
            UserManager<AppUser> userManager,
            ICurrentUserService currentUserService,
            ISessionService sessionService)
        {
            _userManager = userManager;
            _currentUserService = currentUserService;
            _sessionService = sessionService;
        }

        public async Task<RequestResult<Unit>> Handle(
            ChangePasswordCommand request,
            CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;

            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user is null)
            {
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

                return RequestResult<Unit>.Failure(code);
            }
            await _sessionService.RevokeAllSessionsAsync(user.Id, cancellationToken);

            return RequestResult<Unit>.succeeded(Unit.Value, ResultCode.PasswordChangedSuccessfully);
        }
    }
}



