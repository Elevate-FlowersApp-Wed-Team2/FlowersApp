using FlowersApp.Auth.Domain.Entities;
using FlowersApp.Auth.Infrastructure.Persistence;
using FlowersApp.Auth.Shared.Response;
using FlowersApp.Auth.Shared.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MassTransit;

namespace FlowersApp.Auth.Features.CustomerRegister;

public class CustomerRegisterHandler : ICommandHandler<CustomerRegisterCommand, Guid>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly AppDbContext _db;
    private readonly IPublishEndpoint _publishEndpoint;

    public CustomerRegisterHandler(
        UserManager<AppUser> userManager,
        RoleManager<Role> roleManager,
        AppDbContext db , IPublishEndpoint publishEndpoint)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _db = db;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<RequestResult<Guid>> Handle(CustomerRegisterCommand request, CancellationToken cancellationToken)
    {
        // Check duplicates
        var existingByEmail = await _userManager.FindByEmailAsync(request.Email);
        if (existingByEmail is not null)
        {
            return RequestResult<Guid>.Failure(ResultCode.EmailAlreadyRegistered);
        }

        var existingByPhone = await _userManager.Users
            .FirstOrDefaultAsync(u => u.PhoneNumber == request.PhoneNumber, cancellationToken);
        if (existingByPhone is not null)
        {
            return RequestResult<Guid>.Failure(ResultCode.PhoneAlreadyRegistered);
        }

        var user = new FlowersApp.Auth.Domain.Entities.Customer
        {
            UserName = request.Email,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            FullName = request.FullName,
            Gender = request.Gender,
            CreatedAt = DateTime.UtcNow
        };

        // Use a transaction to avoid partial state
        using var tx = await _db.Database.BeginTransactionAsync(cancellationToken);
        var createResult = await _userManager.CreateAsync(user, request.Password);

        if (!createResult.Succeeded)
        {
            var errors = string.Join(
                " | ",
                createResult.Errors.Select(e => $"{e.Code}: {e.Description}")
            );

            throw new InvalidOperationException(
                $"User creation failed: {errors}");
        }

        // Ensure role exists
        const string customerRole = "Customer";
        if (!await _roleManager.RoleExistsAsync(customerRole))
        {
            var roleCreate = await _roleManager.CreateAsync(new Role { Name = customerRole });
            if (!roleCreate.Succeeded)
            {
                return RequestResult<Guid>.Failure(ResultCode.RegistrationFailed);
            }
        }

        var addToRole = await _userManager.AddToRoleAsync(user, customerRole);
        if (!addToRole.Succeeded)
        {
            return RequestResult<Guid>.Failure(ResultCode.RegistrationFailed);
        }

        await tx.CommitAsync(cancellationToken);
        await _publishEndpoint.Publish(new CustomerRegisterEvent(user.Id.ToString(), user.Email), cancellationToken);

        return RequestResult<Guid>.succeeded(user.Id, ResultCode.RegistrationSuccessful);
    }
}
