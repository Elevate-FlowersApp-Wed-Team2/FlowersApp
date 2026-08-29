using FluentValidation;

namespace FlowersApp.Notification.Features.Notifications.SendAdminNotification;

public class SendAdminNotificationValidator : AbstractValidator<SendAdminNotificationCommand>
{
    public SendAdminNotificationValidator()
    {
        RuleFor(x => x.Type)
            .IsInEnum()
            .WithMessage("Invalid NotificationType.");

        RuleFor(x => x.Translations)
            .NotEmpty()
            .WithMessage("At least one translation (Title/Body) must be provided.");

        RuleForEach(x => x.Translations).ChildRules(t =>
        {
            t.RuleFor(x => x.Language).NotEmpty().WithMessage("Language is required.");
            t.RuleFor(x => x.Title).NotEmpty().WithMessage("Title is required.");
            t.RuleFor(x => x.Body).NotEmpty().WithMessage("Body is required.");
        });
    }
}
