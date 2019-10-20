using System;
using System.Collections.Generic;
using System.Text;

namespace ProTickDatabase.DatabasePOCOs
{
    public class EmployeeTeam
    {
        public int EmployeeTeamID { get; set; }

        public virtual Employee Employee { get; set; }
        public virtual Team Team { get; set; }
    }
}
