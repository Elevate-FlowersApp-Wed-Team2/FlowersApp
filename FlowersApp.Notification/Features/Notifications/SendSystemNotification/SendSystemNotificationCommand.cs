using FlowersApp.Notification.Domain.Enums;
using FlowersApp.Notification.Shared.Interfaces;

namespace FlowersApp.Notification.Features.Notifications.SendSystemNotification;

public class NotificationTranslationDto
{
    public string Language { get; set; } = "en";
    public string Title { get; set; } = null!;
    public string Body { get; set; } = null!;
}

public class SendSystemNotificationCommand : ICommand<Guid>
{
    public Guid UserId { get; set; }
    public NotificationType Type { get; set; }
    public string? Payload { get; set; }
    public List<NotificationTranslationDto> Translations { get; set; } = new();
}
