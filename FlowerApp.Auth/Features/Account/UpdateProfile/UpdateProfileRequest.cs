using FlowerApp.Auth.Domain.Enums;

namespace FlowerApp.Auth.Features.Account.UpdateProfile
{
    public class UpdateProfileRequest
    {
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
        public Gender Gender { get; set; }
        public IFormFile? ProfilePhoto { get; set; } // optional 
    }
}
