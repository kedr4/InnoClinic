using MediatR;

namespace Business.Commands.CreateOffice;

public class CreateOfficeCommand : IRequest<Guid>
{
    public string City { get; set; } = string.Empty;
    public string Street { get; set; } = string.Empty;
    public string HouseNumber { get; set; } = string.Empty;
    public string OfficeNumber { get; set; } = string.Empty;
    public string? PhotoUrl { get; set; }
    public string RegistryPhoneNumber { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
}
