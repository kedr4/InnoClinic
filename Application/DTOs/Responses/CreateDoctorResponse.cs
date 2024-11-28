namespace Application.DTOs.Responses;

public record CreateDoctorResponse(
    bool Success = false,
    string Message = "",
    Guid UserId = default
    );