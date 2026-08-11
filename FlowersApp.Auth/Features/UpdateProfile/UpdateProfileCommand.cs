using FlowersApp.Auth.Domain.Enums;
using FlowersApp.Auth.Shared.Interfaces;

namespace FlowersApp.Auth.Features.UpdateProfile
{
    public sealed record PhotoUpload(Stream Stream, string FileName, string ContentType);

    public class UpdateProfileCommand : ICommand<UpdateProfileResponse>
    {
        public string? FullName { get; set; }
        public Gender? Gender { get; set; }
        public string? PhoneNumber { get; set; }
        public PhotoUpload? Photo { get; set; }   
    }
}
