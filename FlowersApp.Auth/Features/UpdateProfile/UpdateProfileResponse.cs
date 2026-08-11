using FlowersApp.Auth.Domain.Enums;

namespace FlowersApp.Auth.Features.UpdateProfile
{
    public class UpdateProfileResponse
    {
        public string FullName { get; set; } = default!;
        public Gender Gender { get; set; }
        public string? PhoneNumber { get; set; }
        public string? ProfilePhotoUrl { get; set; }
    }
}
