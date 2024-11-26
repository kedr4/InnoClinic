using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Requests;

public class UpdateDoctorRequest
{
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string MiddleName { get; init; }
    public DateOnly DateOfBirth { get; init; }
    public Guid SpecializationId { get; init; }
    public Guid OfficeId { get; init; }
    public int CareerStartYear { get; init; }

    public UpdateDoctorRequest(string firstName, string lastName, string middleName, DateOnly dateOfBirth,
                               Guid specializationId, Guid officeId, int careerStartYear)
    {
        FirstName = firstName;
        LastName = lastName;
        MiddleName = middleName;
        DateOfBirth = dateOfBirth;
        SpecializationId = specializationId;
        OfficeId = officeId;
        CareerStartYear = careerStartYear;
    }
}
