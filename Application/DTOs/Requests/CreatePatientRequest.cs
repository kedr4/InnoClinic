using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Requests;

public class CreatePatientRequest
{
    [Required(ErrorMessage = "Please, enter the email")]
    [EmailAddress(ErrorMessage = "You've entered an invalid email")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Please, enter the password")]
    [StringLength(15, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 15 characters.")]
    public string Password { get; set; }

    [Required(ErrorMessage = "Please, reenter the password")]
    [StringLength(15, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 15 characters.")]
    [Compare("Password", ErrorMessage = "The passwords you've entered don't coincide.")]
    public string ConfirmPassword { get; set; }

    [Required(ErrorMessage = "Please, enter your first name")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "Please, enter your last name")]
    public string LastName { get; set; }

    [Required(ErrorMessage = "Please, enter your middle name")]
    public string MiddleName { get; set; }

    public DateOnly DateOfBirth { get; set; }

    public bool IsLinkedToAccount { get; set; }

    public CreatePatientRequest(string email, string password, string firstName, string lastName, string middleName, DateOnly dateOfBirth, bool isLinkedToAccount)
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

  
