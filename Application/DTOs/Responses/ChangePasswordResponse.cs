namespace Application.DTOs.Responses;

public record ChangePasswordResponse
(
    bool Success = false,
    string Message = "",
    Guid UserId = default
);