using FlowersApp.Notification.Domain.Enums;
using FlowersApp.Notification.Features.Notifications.SendSystemNotification;
using FlowersApp.Notification.Shared.Interfaces;

namespace FlowersApp.Notification.Features.Notifications.SendAdminNotification;

public class SendAdminNotificationCommand : ICommand<Guid>
{
    public Guid? TargetUserId { get; set; }
    public NotificationType Type { get; set; } = NotificationType.Promotional;
    public string? Payload { get; set; }
    public List<NotificationTranslationDto> Translations { get; set; } = new();
}
