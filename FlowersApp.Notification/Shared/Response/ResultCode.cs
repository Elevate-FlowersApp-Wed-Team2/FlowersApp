namespace FlowersApp.Notification.Shared.Response;

public enum ResultCode
{
    // Device Registration (800 - 809)
    DeviceRegisteredSuccessfully = 800,
    DeviceNotFound = 801,
    InvalidDeviceData = 802,
    DeviceDeactivated = 803,

    // Notification Sending & Delivery (810 - 829)
    NotificationSentSuccessfully = 810,
    NotificationFailed = 811,
    NotificationNotFound = 812,
    NotificationQueued = 813,
    InvalidNotificationData = 814,
    NoActiveDevicesFound = 815
}
