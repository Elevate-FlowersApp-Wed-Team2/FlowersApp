using FlowerApp.Auth.Domain.Enums;

namespace FlowerApp.Auth.Features.Account.UpdateProfile
{
    public record UpdateProfileResponse(
     string FirstName,
     string LastName,
     string PhoneNumber,
     Gender Gender,
     string? ProfilePhotoUrl
    );
}
