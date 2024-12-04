namespace Application.DTOs.Responses;

public record ChangePasswordResponse
(
    bool Success,
    string Message,
    Guid UserId
);