using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Receptionist:IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        public string LastName {    get; set; }
        public string MiddleName { get; set; }
        public DateOnly DateOfBirth { get; set; }
        //
        public Guid AccountId { get; set; }
        public Guid OfficeId { get; set; }
    }
}
