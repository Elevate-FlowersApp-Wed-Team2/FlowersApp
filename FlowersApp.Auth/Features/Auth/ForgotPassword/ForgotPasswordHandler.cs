using FlowersApp.Auth.Domain.Entities;
using FlowersApp.Auth.Features.Auth.ForgotPassword.Events;
using FlowersApp.Auth.Shared.Interfaces;
using FlowersApp.Auth.Shared.Response;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;

namespace FlowersApp.Auth.Features.Auth.ForgotPassword;

public class ForgotPasswordHandler : ICommandHandler<ForgotPasswordCommand, object?>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IPasswordResetOtpService _otpService;
    private readonly IPublisher _publisher;
    private readonly ILogger<ForgotPasswordHandler> _logger;

    public ForgotPasswordHandler(
        UserManager<AppUser> userManager,
        IPasswordResetOtpService otpService,
        IPublisher publisher,
        ILogger<ForgotPasswordHandler> logger)
    {
        _userManager = userManager;
        _otpService = otpService;
        _publisher = publisher;
        _logger = logger;
    }

    public async Task<RequestResult<object?>> Handle(
        ForgotPasswordCommand request,
        CancellationToken cancellationToken)
    {
        var email = request.Email.Trim();

        // Cooldown is email-keyed for both known and unknown accounts (no enumeration).
        var (allowed, _) = await _otpService.TryBeginIssueByEmailAsync(email, cancellationToken);
        if (!allowed)
            return RequestResult<object?>.Failure(ResultCode.OtpResendTooSoon);

        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
        {
            _logger.LogInformation("Forgot-password requested for unknown email.");
            // Same success path as a real send — cooldown already applied.
            return RequestResult<object?>.succeeded(null, ResultCode.PasswordResetOtpSent);
        }

        var otp = RandomNumberGenerator.GetInt32(0, 1_000_000).ToString("D6");
        await _otpService.StoreOtpAsync(user.Id, otp, cancellationToken);

        await _publisher.Publish(
            new PasswordResetOtpRequestedEvent(user.Id, user.Email!, otp),
            cancellationToken);

        _logger.LogInformation("Password reset OTP issued for user {UserId}.", user.Id);
        return RequestResult<object?>.succeeded(null, ResultCode.PasswordResetOtpSent);
    }
}
