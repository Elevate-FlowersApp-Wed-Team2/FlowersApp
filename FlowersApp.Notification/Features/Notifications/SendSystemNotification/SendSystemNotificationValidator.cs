using FluentValidation;

namespace FlowersApp.Notification.Features.Notifications.SendSystemNotification;

public class SendSystemNotificationValidator : AbstractValidator<SendSystemNotificationCommand>
{
    public SendSystemNotificationValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("UserId is required.");

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
