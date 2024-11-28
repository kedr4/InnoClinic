namespace Application.DTOs.Responses;

public record CreatePatientResponse(
    bool Success = false,
    string Message = "",
    Guid UserId = default
    );