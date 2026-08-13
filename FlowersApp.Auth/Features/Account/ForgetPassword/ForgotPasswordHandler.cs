using FlowerApp.Auth.Domain.Interfaces;
using FlowersApp.Auth.Domain.Entities;
using FlowersApp.Auth.Shared.Interfaces;
using FlowersApp.Auth.Shared.Response;
using Microsoft.AspNetCore.Identity;

namespace FlowersApp.Auth.Features.Account.ForgetPassword
{
    public class ForgotPasswordHandler : ICommandHandler<ForgotPasswordCommand, ForgotPasswordResponse>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IPasswordResetOtpService _otpService;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<ForgotPasswordHandler> _logger;

        public ForgotPasswordHandler(
            UserManager<AppUser> userManager,
            IPasswordResetOtpService otpService,
            IEmailSender emailSender,
            ILogger<ForgotPasswordHandler> logger)
        {
            _userManager = userManager;
            _otpService = otpService;
            _emailSender = emailSender;
            _logger = logger;
        }

        public async Task<RequestResult<ForgotPasswordResponse>> Handle(
            ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            const string neutralMessage = "If this email is registered, a verification code has been sent.";

            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user is not null && !await _otpService.IsResendOnCooldownAsync(request.Email))
            {
                var otp = await _otpService.GenerateOtpAsync(request.Email);

                try
                {
                    await _emailSender.SendAsync(
                        request.Email,
                        "Your password reset code",
                        $"Your verification code is {otp}. It expires in 10 minutes.",
                        cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to send password reset OTP to {Email}", request.Email);
                }
            }

            // AC #2 — always the same neutral outcome, never reveals whether the account exists.
            return RequestResult<ForgotPasswordResponse>.succeeded(
                new ForgotPasswordResponse(neutralMessage), ResultCode.OtpSent);
        }
    }
}
