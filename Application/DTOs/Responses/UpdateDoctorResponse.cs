namespace Application.DTOs.Responses;

public record UpdateDoctorResponse
(
    bool Success,
    string Message,
    Guid UserId
);