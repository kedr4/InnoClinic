namespace Application.DTOs.Responses;

public record UpdateDoctorResponse
(
    bool Success = false,
    string Message = "",
    Guid UserId = default
);