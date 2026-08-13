using FlowerApp.Auth.Domain.Interfaces;
using FlowersApp.Auth.Domain.Entities;
using FlowersApp.Auth.Shared.Interfaces;
using FlowersApp.Auth.Shared.Response;
using Microsoft.AspNetCore.Identity;

namespace FlowersApp.Auth.Features.Account.ResetPassword
{
    public class ResetPasswordHandler : ICommandHandler<ResetPasswordOrchestrator, ResetPasswordResponse>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IPasswordResetOtpService _otpService;
        private readonly ISessionService _sessionService; 
        private readonly ILogger<ResetPasswordHandler> _logger;

        public ResetPasswordHandler(
            UserManager<AppUser> userManager,
            IPasswordResetOtpService otpService,
            ISessionService sessionService,
            ILogger<ResetPasswordHandler> logger)
        {
            _userManager = userManager;
            _otpService = otpService;
            _sessionService = sessionService;
            _logger = logger;
        }

        public async Task<RequestResult<ResetPasswordResponse>> Handle(
            ResetPasswordOrchestrator request, CancellationToken cancellationToken)
        {
            var tokenValid = await _otpService.ValidateResetTokenAsync(request.Email, request.ResetToken);
            if (!tokenValid)
             return RequestResult<ResetPasswordResponse>.Failure(
                 
                 ResultCode.ResetTokenInvalid);

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
                return RequestResult<ResetPasswordResponse>.Failure(ResultCode.UserNotFound);

            var identityToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetResult = await _userManager.ResetPasswordAsync(user, identityToken, request.NewPassword);

            if (!resetResult.Succeeded)
            {
                var errors = string.Join(", ", resetResult.Errors.Select(e => e.Description));
                _logger.LogWarning("Password reset failed for {Email}: {Errors}", request.Email, errors);
                return RequestResult<ResetPasswordResponse>.Failure(ResultCode.PasswordChangeFailed);
            }

            await _otpService.InvalidateResetTokenAsync(request.Email);

            // invalidate all existing sessions/refresh tokens.
            await _sessionService.RevokeAllSessionsAsync(user.Id);

            return RequestResult<ResetPasswordResponse>.succeeded(
                new ResetPasswordResponse("Your password has been reset successfully. Please sign in."),
                ResultCode.PasswordResetSuccessfully);
        }
    }
}
