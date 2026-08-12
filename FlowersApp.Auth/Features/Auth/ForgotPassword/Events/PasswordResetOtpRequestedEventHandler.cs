using FlowerApp.Auth.Domain.Interfaces;
using MediatR;

namespace FlowersApp.Auth.Features.Auth.ForgotPassword.Events;

public class PasswordResetOtpRequestedEventHandler : INotificationHandler<PasswordResetOtpRequestedEvent>
{
    private readonly IEmailSender _emailSender;
    private readonly ILogger<PasswordResetOtpRequestedEventHandler> _logger;

    public PasswordResetOtpRequestedEventHandler(
        IEmailSender emailSender,
        ILogger<PasswordResetOtpRequestedEventHandler> logger)
    {
        _emailSender = emailSender;
        _logger = logger;
    }

    public async Task Handle(PasswordResetOtpRequestedEvent notification, CancellationToken cancellationToken)
    {
        try
        {
            await _emailSender.SendAsync(
                toEmail: notification.Email,
                subject: "Your FlowersApp password reset code",
                body: $"Your verification code is {notification.Otp}. It expires in 10 minutes.",
                cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex,
                "Failed to send password-reset OTP email for user {UserId}.",
                notification.UserId);
        }
    }
}
