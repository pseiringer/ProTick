using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ProTick.ResourceDTOs
{
    public class EmployeeDTO
    {
        public int EmployeeID { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        public string DateOfBirth { get; set; }
        public string HireDate { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public int AddressID { get; set; }

    }
}
