using FlowersApp.Auth.Domain.Entities;
using FlowersApp.Auth.Infrastructure.Persistence.Repositories;
using FlowersApp.Auth.Shared.Interfaces;
using FlowersApp.Auth.Shared.Response;
using Microsoft.EntityFrameworkCore;

namespace FlowersApp.Auth.Features.Vehicles.CheckVehicleExistenceByVehicleNumber;

public record CheckVehicleExistenceByVehicleNumberQuery(string VehicleNumber) : IQuery<bool>;

public class CheckVehicleExistenceByVehicleNumberQueryHandler(Repository<Vehicle> repository)
    : IQueryHandler<CheckVehicleExistenceByVehicleNumberQuery, bool>
{
    private readonly Repository<Vehicle> _repository = repository;

    public async Task<RequestResult<bool>> Handle(CheckVehicleExistenceByVehicleNumberQuery request, CancellationToken cancellationToken)
    {
        var exists = await _repository.Get()
            .AnyAsync(v => v.VehicleNumber == request.VehicleNumber ,cancellationToken);

        return RequestResult<bool>.succeeded(
            exists,
            ResultCode.VehicleExistenceChecked);
    }
}