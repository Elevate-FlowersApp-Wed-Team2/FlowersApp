using FlowersApp.Auth.Domain.Enums;

namespace FlowersApp.Auth.Domain.Entities;

public class DriverApplication: BaseEntity
{
    public DriverApplicationStatus Status { get; set; }
    public string? RejectReason { get; set; }
    public Guid ReviewedBy { get; set; }
    public DateTime ReviewedAt { get; set; }
    public string FullName { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public Gender Gender { get; set; }
    public required string NationalIDNumber { get; set; }
    public Guid VehicleID { get; set; }
    public required string HashedPassword { get; set; }
    public List<DriverDocument>? Documents { get; set; }
    public Vehicle? Vehicle { get; set; }
}
