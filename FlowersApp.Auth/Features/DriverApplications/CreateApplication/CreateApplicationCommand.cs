using FlowersApp.Auth.Domain.Entities;
using FlowersApp.Auth.Domain.Enums;
using FlowersApp.Auth.Infrastructure.Persistence.Repositories;
using FlowersApp.Auth.Shared.Interfaces;
using FlowersApp.Auth.Shared.Response;
using Microsoft.AspNetCore.Identity;

namespace FlowersApp.Auth.Features.DriverApplications.CreateApplication;

public record CreateApplicationCommand
(string Name,
    string Email,
    string Password,
    string Phone,
    Gender Gender,
    Guid VehicleId,
    string VehicleNumber,
    string LicenceNumber,
    string Nid,
    string? FcmToken = null) : ICommand<Guid>;

public class CreateApplicationCommandHandler(Repository<DriverApplication> repository ,
    IPasswordHasher<AppUser> passwordHasher) : ICommandHandler<CreateApplicationCommand, Guid>
{
    private readonly Repository<DriverApplication> _repository = repository;
    private readonly IPasswordHasher<AppUser> _passwordHasher = passwordHasher;

    public async Task<RequestResult<Guid>> Handle(CreateApplicationCommand request, CancellationToken cancellationToken)
    {
        var application = new DriverApplication
        {
            Id = Guid.NewGuid(),
            FullName = request.Name,
            Email = request.Email,
            PhoneNumber = request.Phone,
            NationalIDNumber = request.Nid,
            VehicleID = request.VehicleId,
            Status = DriverApplicationStatus.Pending,
            LicenseNumber = request.LicenceNumber,
            Gender = request.Gender,
            FcmToken = request.FcmToken,
        };
        var hashedPassword = _passwordHasher.HashPassword(new AppUser(), request.Password);
        application.HashedPassword = hashedPassword;
        _repository.Add(application);
        return RequestResult<Guid>.succeeded(application.Id, ResultCode.ApplicationCreatedSuccessfully);
    }
}