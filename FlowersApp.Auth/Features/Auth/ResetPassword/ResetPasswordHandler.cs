using FlowerApp.Auth.Domain.Interfaces;
using FlowersApp.Auth.Domain.Entities;
using FlowersApp.Auth.Features.Customer.ChangePassword.Events;
using FlowersApp.Auth.Shared.Interfaces;
using FlowersApp.Auth.Shared.Response;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FlowersApp.Auth.Features.Auth.ResetPassword;

public class ResetPasswordHandler : ICommandHandler<ResetPasswordCommand, object?>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IPasswordResetOtpService _otpService;
    private readonly ISessionService _sessionService;
    private readonly IPublisher _publisher;
    private readonly ILogger<ResetPasswordHandler> _logger;

    public ResetPasswordHandler(
        UserManager<AppUser> userManager,
        IPasswordResetOtpService otpService,
        ISessionService sessionService,
        IPublisher publisher,
        ILogger<ResetPasswordHandler> logger)
    {
        _userManager = userManager;
        _otpService = otpService;
        _sessionService = sessionService;
        _publisher = publisher;
        _logger = logger;
    }

    public async Task<RequestResult<object?>> Handle(
        ResetPasswordCommand request,
        CancellationToken cancellationToken)
    {
        var userId = await _otpService.ConsumeResetTokenAsync(request.ResetToken, cancellationToken);
        if (userId is null)
            return RequestResult<object?>.Failure(ResultCode.InvalidOrExpiredResetToken);

        var user = await _userManager.FindByIdAsync(userId.Value.ToString());
        if (user is null)
            return RequestResult<object?>.Failure(ResultCode.UserNotFound);

        var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
        var identityResult = await _userManager.ResetPasswordAsync(user, resetToken, request.NewPassword);
        if (!identityResult.Succeeded)
        {
            _logger.LogError(
                "Password reset failed for user {UserId}: {Errors}",
                user.Id,
                string.Join(", ", identityResult.Errors.Select(e => e.Description)));
            return RequestResult<object?>.Failure(ResultCode.PasswordChangeFailed);
        }

        await _sessionService.RevokeAllSessionsAsync(user.Id, cancellationToken);
        await _publisher.Publish(new PasswordChangedEvent(user.Id, user.Email!), cancellationToken);

        _logger.LogInformation("Password reset completed for user {UserId}.", user.Id);
        return RequestResult<object?>.succeeded(null, ResultCode.PasswordResetSuccessful);
    }
}
