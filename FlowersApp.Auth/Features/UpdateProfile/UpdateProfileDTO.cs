using FlowersApp.Auth.Domain.Enums;

namespace FlowersApp.Auth.Features.UpdateProfile
{
    public class UpdateProfileDTO
    {
        public string? FullName { get; set; }
        public Gender? Gender { get; set; }
        public string? PhoneNumber { get; set; }
        public IFormFile? ProfilePhoto { get; set; }
    }

}
