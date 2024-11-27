namespace Application.DTOs.Responses;

public record CreatePatientResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public Guid UserId { get; set; }
}