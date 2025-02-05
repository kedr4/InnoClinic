using MediatR;
using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;

namespace Business.Offices.Commands.CreateOffice;

public class CreateOfficeCommand : IRequest<Guid>
{

    [JsonPropertyName("userId")]
    public Guid UserId { get; set; }

    [JsonPropertyName("city")]
    public string City { get; set; }

    [JsonPropertyName("street")]
    public string Street { get; set; }

    [JsonPropertyName("houseNumber")]
    public string HouseNumber { get; set; }

    [JsonPropertyName("officeNumber")]
    public string OfficeNumber { get; set; }

    [JsonPropertyName("photo")]
    public IFormFile? Photo { get; set; }

    [JsonPropertyName("registryPhoneNumber")]
    public string RegistryPhoneNumber { get; set; }

    [JsonPropertyName("isActive")]
    public bool IsActive { get; set; } = true;

    public CreateOfficeCommand(Guid userId, string city, string street, string houseNumber,
           string officeNumber, IFormFile? photo, string registryPhoneNumber, bool isActive = true)
    {
        UserId = userId;
        City = city;
        Street = street;
        HouseNumber = houseNumber;
        OfficeNumber = officeNumber;
        Photo = photo;
        RegistryPhoneNumber = registryPhoneNumber;
        IsActive = isActive;
    }
}