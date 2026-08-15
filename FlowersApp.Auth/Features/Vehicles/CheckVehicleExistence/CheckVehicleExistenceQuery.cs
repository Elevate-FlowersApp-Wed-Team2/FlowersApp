using FlowersApp.Auth.Domain.Entities;
using FlowersApp.Auth.Infrastructure.Persistence.Repositories;
using FlowersApp.Auth.Shared.Interfaces;
using FlowersApp.Auth.Shared.Response;
using Microsoft.EntityFrameworkCore;

namespace FlowersApp.Auth.Features.Vehicles.CheckVehicleExistence;

public record CheckVehicleExistenceQuery(Guid VehicleId)
    :IQuery<bool>;

public class CheckVehicleExistenceQueryHandler(Repository<Vehicle> repository) : IQueryHandler<CheckVehicleExistenceQuery, bool>
{
    private readonly Repository<Vehicle> _repository = repository;

    public async Task<RequestResult<bool>> Handle(CheckVehicleExistenceQuery request, CancellationToken cancellationToken)
    {
        var isExist = await _repository.Get(d => d.Id == request.VehicleId)
                                          .AnyAsync(cancellationToken);
        if(isExist) 
            return RequestResult<bool>.succeeded(isExist, ResultCode.VehicleExist);
        return RequestResult<bool>.succeeded(isExist, ResultCode.VehicleNotFound);
    }
}
