﻿namespace Application.DTOs.Requests;

public class CreateDoctorRequest
{
    public string Email { get; init; }
    public string Password { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string MiddleName { get; init; }
    public DateOnly DateOfBirth { get; init; }
    public Guid SpecializationId { get; init; }
    public Guid OfficeId { get; init; }
    public int CareerStartYear { get; init; }

    public CreateDoctorRequest(string email, string password, string firstName, string lastName, string middleName,
                               DateOnly dateOfBirth, Guid specializationId, Guid officeId, int careerStartYear)
    {
        Email = email;
        Password = password;
        FirstName = firstName;
        LastName = lastName;
        MiddleName = middleName;
        DateOfBirth = dateOfBirth;
        SpecializationId = specializationId;
        OfficeId = officeId;
        CareerStartYear = careerStartYear;
    }
}
