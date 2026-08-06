using FlowersApp.Auth.Domain.Enums;

namespace FlowersApp.Auth.Domain.Entities;

public class Vehicle:BaseEntity
{
    public required string VehicleNumber { get; set; }
    public required VehicleType Type { get; set; }
    public Guid? DriverId { get; set; }
    public Driver? Driver { get; set; }
}
