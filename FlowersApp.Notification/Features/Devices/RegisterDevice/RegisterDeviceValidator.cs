using FluentValidation;

namespace FlowersApp.Notification.Features.Devices.RegisterDevice;

public class RegisterDeviceValidator : AbstractValidator<RegisterDeviceCommand>
{
    public RegisterDeviceValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("UserId is required.");

        RuleFor(x => x.DeviceId)
            .NotEmpty()
            .WithMessage("DeviceId is required.");

        RuleFor(x => x.FcmToken)
            .NotEmpty()
            .WithMessage("FcmToken is required.");

        RuleFor(x => x.Language)
            .NotEmpty()
            .WithMessage("Language is required.");
    }
}
