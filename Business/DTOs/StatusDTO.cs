using DataAccess.DTOs;
using DataAccess.Models;
using System.Text.Json.Serialization;

namespace Business.DTOs;
public class StatusDTO
{
    [JsonPropertyName("_id")]
    public Guid Id { get; set; }

    [JsonPropertyName("statusType")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public StatusType StatusType { get; set; }

    [JsonPropertyName("officeDto")]
    public OfficeDTO OfficeDTO { get; set; }

    [JsonPropertyName("errorMessage")]
    public string? ErrorMessage { get; set; } = string.Empty;

    public static StatusDTO MapFromModel(Status status, OfficeDTO officeDTO)
    {
        return new StatusDTO
        {
            Id = status.Id,
            StatusType = status.StatusType,
            OfficeDTO = officeDTO,
            ErrorMessage = status.ErrorMessage
        };
    }

    public static StatusDTO MapFromModel(Status status, Office office)
    {
        return new StatusDTO
        {
            Id = status.Id,
            StatusType = status.StatusType,
            OfficeDTO = OfficeDTO.MapFromModel(office),
            ErrorMessage = status.ErrorMessage
        };
    }
}
