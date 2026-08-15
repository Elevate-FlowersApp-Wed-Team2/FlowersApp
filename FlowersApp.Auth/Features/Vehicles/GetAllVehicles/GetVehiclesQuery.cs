using FlowersApp.Auth.Domain.Entities;
using FlowersApp.Auth.Domain.Enums;
using FlowersApp.Auth.Infrastructure.Persistence.Repositories;
using FlowersApp.Auth.Shared.Interfaces;
using FlowersApp.Auth.Shared.Response;
using Microsoft.EntityFrameworkCore;




namespace FlowersApp.Auth.Features.Vehicles.GetAllVehicles;

public record GetVehiclesQuery() : IQuery<List<VehicleListItem>>;

public record VehicleListItem(Guid Id, string VehicleNumber, VehicleType Type);

public class GetVehiclesQueryHandler(Repository<Vehicle> repository)
    : IQueryHandler<GetVehiclesQuery, List<VehicleListItem>>
{
    private readonly Repository<Vehicle> _repository = repository;

    public async Task<RequestResult<List<VehicleListItem>>> Handle(GetVehiclesQuery request, CancellationToken cancellationToken)
    {
        var vehicles = await _repository.Get()
            .Select(v => new VehicleListItem(v.Id, v.VehicleNumber, v.Type))
            .ToListAsync(cancellationToken);

        return RequestResult<List<VehicleListItem>>.succeeded(vehicles, ResultCode.VehiclesRetrieved);
    }
}