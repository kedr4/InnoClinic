namespace Application.DTOs.Responses;

public record UpdateReceptionistResponse
    (
    bool Success = false,
    string Message = "",
    Guid UserId = default
    );