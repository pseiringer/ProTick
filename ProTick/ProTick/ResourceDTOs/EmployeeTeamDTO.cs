using System;
using System.Collections.Generic;
using System.Text;

namespace ProTick.ResourceDTOs
{
    public class EmployeeTeamDTO
    {
        public int EmployeeTeamID { get; set; }
        public string Role { get; set; }

        public int EmployeeID { get; set; }
        public int TeamID { get; set; }
    }
}
