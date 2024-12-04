namespace Application.DTOs.Responses;

public record UpdatePatientResponse
(
    bool Success,
    string Message,
    Guid UserId
);