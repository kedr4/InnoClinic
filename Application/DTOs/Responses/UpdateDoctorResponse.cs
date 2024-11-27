namespace Application.DTOs.Responses;

public record UpdateDoctorResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public Guid UserId { get; set; }
}
