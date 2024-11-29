namespace Application.DTOs.Responses;

public record CreateReceptionistResponse
    (
    bool Success = false,
    string Message = "",
    Guid ReceptionistId = default
    );