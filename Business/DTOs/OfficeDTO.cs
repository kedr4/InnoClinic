using DataAccess.Models;
using System.Text.Json.Serialization;

namespace Business.DTOs
{
    public record OfficeDTO
    {
        [JsonPropertyName("_id")]
        public Guid Id { get; init; }

        [JsonPropertyName("city")]
        public string City { get; init; } = string.Empty;

        [JsonPropertyName("street")]
        public string Street { get; init; } = string.Empty;

        [JsonPropertyName("houseNumber")]
        public string HouseNumber { get; init; } = string.Empty;

        [JsonPropertyName("officeNumber")]
        public string OfficeNumber { get; init; } = string.Empty;

        [JsonPropertyName("address")]
        public string Address { get; init; } = string.Empty;

        [JsonPropertyName("photo")]
        public string? Photo { get; init; }

        [JsonPropertyName("registryPhoneNumber")]
        public string RegistryPhoneNumber { get; init; } = string.Empty;

        [JsonPropertyName("isActive")]
        public bool IsActive { get; init; } = true;

        public static OfficeDTO MapFromModel(Office office)
        {
            return new OfficeDTO
            {
                Id = office.Id,
                City = office.City,
                Street = office.Street,
                HouseNumber = office.HouseNumber,
                OfficeNumber = office.OfficeNumber,
                Address = office.Address,
                Photo = office.Photo,
                RegistryPhoneNumber = office.RegistryPhoneNumber,
                IsActive = office.IsActive
            };
        }
    }
}
