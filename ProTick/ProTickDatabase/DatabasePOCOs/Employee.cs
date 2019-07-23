using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ProTickDatabase.DatabasePOCOs
{
    public class Employee
    {
        public int EmployeeID { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public DateTime DateOfBirth { get; set; }
        public DateTime HireDate { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public virtual Address Address { get; set; }

    }
}
