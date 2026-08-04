using FlowersApp.Auth.Domain.Enums;

namespace FlowersApp.Auth.Domain.Entities;

public class Driver :AppUser
{
    public required string NationalIDNumber { get; set; }
    public Guid VehicleId { get; set; }
    public DriverStatus DriverStatus { get; set; }
    public Guid? DriverApplicationId { get; set; }
    public DriverApplication? DriverApplication { get; set; }
    public Vehicle? Vehicle { get; set; }
}
