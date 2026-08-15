using FlowersApp.Auth.Domain.Entities;
using FlowersApp.Auth.Domain.Enums;
using FlowersApp.Auth.Features.Vehicles.CheckVehicleExistence;
using FlowersApp.Auth.Features.Vehicles.CheckVehicleExistenceByVehicleNumber;
using FlowersApp.Auth.Infrastructure.Persistence.Repositories;
using FlowersApp.Auth.Shared.Interfaces;
using FlowersApp.Auth.Shared.Response;
using MediatR;
using Microsoft.AspNetCore.Components.Forms;

namespace FlowersApp.Auth.Features.Vehicles.CreateVehicle;

public record CreateVehicleCommand
    (string VehicleNumber, VehicleType VehicleType) : ICommand<CreateVehicleResponse>;
public record CreateVehicleResponse
    (Guid Id);

public class CreateVehicleCommandHandler(Repository<Vehicle> repository ,IMediator mediator ,ILogger<CreateVehicleCommandHandler> logger)
    : ICommandHandler<CreateVehicleCommand, CreateVehicleResponse>
{
    private readonly Repository<Vehicle> _repository = repository;
    private readonly IMediator _mediator = mediator;
    private readonly ILogger<CreateVehicleCommandHandler> _logger = logger;

    public async Task<RequestResult<CreateVehicleResponse>> Handle(CreateVehicleCommand request, CancellationToken cancellationToken)
    {
        var isExist = await _mediator.Send(new CheckVehicleExistenceByVehicleNumberQuery(request.VehicleNumber));
        if(isExist.Success && isExist.Result)
            return RequestResult<CreateVehicleResponse>.Failure(ResultCode.VehicleAlreadyExists);
        var vehicle = new Vehicle
        {
            Id = Guid.NewGuid(),
            Type = request.VehicleType,
            VehicleNumber = request.VehicleNumber
        };
        _repository.Add(vehicle);
        try
        {
            await _repository.SaveChangeAsync(cancellationToken);
            return RequestResult<CreateVehicleResponse>.succeeded(new CreateVehicleResponse(vehicle.Id), ResultCode.VehicleCreated);
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed To Create Vehicle ex :{}", ex.Message);
            return RequestResult<CreateVehicleResponse>.Failure(ResultCode.FailedToCreateVehicle);
        }
    }
}

