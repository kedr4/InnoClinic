using MediatR;
using System.Text.Json.Serialization;

namespace Business.Offices.Commands.CreateOffice;

public record CreateOfficeCommand(
    [property: JsonPropertyName("city")] string City,
    [property: JsonPropertyName("street")] string Street,
    [property: JsonPropertyName("houseNumber")] string HouseNumber,
    [property: JsonPropertyName("officeNumber")] string OfficeNumber,
    [property: JsonPropertyName("photoUrl")] string? PhotoUrl,
    [property: JsonPropertyName("registryPhoneNumber")] string RegistryPhoneNumber,
    [property: JsonPropertyName("isActive")] bool IsActive = true
) : IRequest<Guid>;