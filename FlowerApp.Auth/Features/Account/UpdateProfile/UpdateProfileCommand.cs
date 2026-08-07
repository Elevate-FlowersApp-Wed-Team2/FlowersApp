using FlowerApp.Auth.Domain.Enums;
using MediatR;
using Shared;

namespace FlowerApp.Auth.Features.Account.UpdateProfile
{
    public record UpdateProfileCommand(
     string FirstName,
     string LastName,
     string PhoneNumber,
     Gender Gender,
     Stream? ProfilePhotoStream,
     string? ProfilePhotoFileName
    ) : IRequest<Result<UpdateProfileResponse>>;

}
