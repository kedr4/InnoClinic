namespace Application.DTOs.Requests;

public record CreatePatientRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string ConfirmPassword { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string MiddleName { get; set; } = string.Empty;
    public DateTimeOffset DateOfBirth { get; set; }

    public bool IsLinkedToAccount { get; set; }

    public CreatePatientRequest(string email, string password, string firstName, string lastName, string middleName, DateTimeOffset dateOfBirth, bool isLinkedToAccount)
    {
        Email = email;
        Password = password;
        FirstName = firstName;
        LastName = lastName;
        MiddleName = middleName;
        DateOfBirth = dateOfBirth;
        IsLinkedToAccount = isLinkedToAccount;
    }
}


