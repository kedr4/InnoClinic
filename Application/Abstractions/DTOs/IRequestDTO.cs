namespace Application.Abstractions.DTOs;

public interface IRequestDTO
{
    public FluentValidation.Results.ValidationResult Validate();
}
