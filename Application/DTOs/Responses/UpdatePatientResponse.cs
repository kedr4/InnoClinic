﻿namespace Application.DTOs.Responses;

public record UpdatePatientResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public Guid UserId { get; set; }
}
