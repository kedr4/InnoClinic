namespace Application.DTOs.Responses;

public record CreateReceptionistReponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public Guid ReceptionistId { get; set; }
}
