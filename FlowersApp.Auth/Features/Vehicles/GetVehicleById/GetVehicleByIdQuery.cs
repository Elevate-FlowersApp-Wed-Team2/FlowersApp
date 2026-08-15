using FlowersApp.Auth.Domain.Entities;
using FlowersApp.Auth.Domain.Enums;
using FlowersApp.Auth.Infrastructure.Persistence.Repositories;
using FlowersApp.Auth.Shared.Interfaces;
using FlowersApp.Auth.Shared.Response;
using Microsoft.EntityFrameworkCore;

namespace FlowersApp.Auth.Features.Vehicles.GetVehicleById;





public record GetVehicleByIdQuery(Guid Id) : IQuery<VehicleDetails>;

public record VehicleDetails(Guid Id, string VehicleNumber, VehicleType Type);

public class GetVehicleByIdQueryHandler(Repository<Vehicle> repository)
    : IQueryHandler<GetVehicleByIdQuery, VehicleDetails>
{
    private readonly Repository<Vehicle> _repository = repository;

    public async Task<RequestResult<VehicleDetails>> Handle(GetVehicleByIdQuery request, CancellationToken cancellationToken)
    {
        var vehicle = await _repository.Get(v => v.Id == request.Id)
            .Select(v => new VehicleDetails(v.Id, v.VehicleNumber, v.Type))
            .FirstOrDefaultAsync(cancellationToken);

        if (vehicle is null)
        {
            return RequestResult<VehicleDetails>.Failure(ResultCode.VehicleNotFound);
        }

        return RequestResult<VehicleDetails>.succeeded(vehicle, ResultCode.VehicleRetrieved);
    }
}