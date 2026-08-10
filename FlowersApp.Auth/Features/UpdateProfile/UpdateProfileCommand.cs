using FlowersApp.Auth.Domain.Enums;
using FlowersApp.Auth.Shared.Interfaces;

namespace FlowersApp.Auth.Features.UpdateProfile
{
    public class UpdateProfileCommand : ICommand<UpdateProfileResponse>
    {
        public string? FullName { get; set; }
        public Gender? Gender { get; set; }
        public string? PhoneNumber { get; set; }
        public Stream? ProfilePhotoStream { get; set; }
        public string? ProfilePhotoFileName { get; set; }
        public string? ProfilePhotoContentType { get; set; }
    }
}
