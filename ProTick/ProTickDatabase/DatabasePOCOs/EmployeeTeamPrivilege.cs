using System;
using System.Collections.Generic;
using System.Text;

namespace ProTickDatabase.DatabasePOCOs
{
    public class EmployeeTeamPrivilege
    {
        public int EmployeeTeamPrivilegeID { get; set; }

        public virtual EmployeeTeam EmployeeTeam { get; set; }
        public virtual Privilege Privilege { get; set; }
    }
}
