namespace Application.DTOs.Responses;

public record UpdatePatientResponse
(
    bool Success = false,
    string Message = "",
    Guid UserId = default
);