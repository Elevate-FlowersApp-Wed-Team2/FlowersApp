using FlowersApp.Auth.Domain.Enums;

namespace FlowersApp.Auth.Domain.Entities;

public class DriverDocument :BaseEntity
{
    public Guid ApplicationId { get; set; }
    public string Name { get; set; }
    public Guid? DriverId { get; set; }
    public DocumentType Type { get; set; }
    public string FileUrl { get; set; }
    public Driver? Driver { get; set; } 
    public DriverApplication DriverApplication { get; set; }
}
