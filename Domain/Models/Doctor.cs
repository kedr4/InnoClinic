using Microsoft.AspNetCore.Identity;

namespace Domain.Models;

public class Doctor : IdentityUser<Guid>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }    
    public string MiddleName { get; set; }
    public DateOnly DateOfBirth { get; set; }     
    //
    public Guid AccountId { get; set; } 
    public Guid SpecializationId { get; set; }
    public Guid OfficeId { get; set; }
    //
    public int CareerStartYear { get; set; }
    public string Status { get; set; }

}