namespace Application.DTOs.Responses;

public record UpdateReceptionistResponse
(
    bool Success,
    string Message,
    Guid UserId
);