using ProTick.ResourceDTOs;
using ProTickDatabase.DatabasePOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace ProTick
{
    public class ResourceDTOConverter
    {
        public AddressDTO AddressToDTO(Address a)
        {
            return new AddressDTO { AddressID = a.AddressID, Country = a.Country, City = a.City, PostalCode = a.PostalCode, Street = a.Street, StreetNumber = a.StreetNumber };
        }

        public EmployeeDTO EmployeeToDTO(Employee a)
        {
            return new EmployeeDTO { AddressID = a.Address.AddressID, DateOfBirth = a.DateOfBirth, EmployeeID = a.EmployeeID, FirstName = a.FirstName, HireDate = a.HireDate, LastName = a.LastName };
        }

        public EmployeeTeamDTO EmployeeTeamToDTO(EmployeeTeam a)
        {
            return new EmployeeTeamDTO { EmployeeID = a.Employee.EmployeeID, EmployeeTeamID = a.EmployeeTeamID, Role = a.Role, TeamID = a.Team.TeamID };
        }

        public EmployeeTeamPrivilegeDTO EmployeeTeamPrivilegeToDTO(EmployeeTeamPrivilege a)
        {
            return new EmployeeTeamPrivilegeDTO { EmployeeTeamID = a.EmployeeTeam.EmployeeTeamID, EmployeeTeamPrivilegeID = a.EmployeeTeamPrivilegeID, PrivilegeID = a.Privilege.PrivilegeID };
        }

        public ParentChildRelationDTO ParentChildRelationToDTO(ParentChildRelation a)
        {
            return new ParentChildRelationDTO { ChildID = a.Child.SubprocessID, ParentChildRelationID = a.ParentChildRelationID, ParentID = a.Parent.SubprocessID};
        }

        public PrivilegeDTO PrivilegeToDTO(Privilege a)
        {
            return new PrivilegeDTO { PrivilegeID = a.PrivilegeID, Description = a.Description };
        }

        public ProcessDTO ProcessToDTO(Process a)
        {
            return new ProcessDTO { Description = a.Description, ProcessID = a.ProcessID };
        }

        public StateDTO StateToDTO(State a)
        {
            return new StateDTO {  Description = a.Description, StateID = a.StateID};
        }

        public SubprocessDTO SubprocessToDTO(Subprocess a)
        {
            return new SubprocessDTO { Description = a.Description, ProcessID = a.Process.ProcessID, SubprocessID = a.SubprocessID, TeamID = a.Team.TeamID};
        }

        public TeamDTO TeamToDTO(Team a)
        {
            return new TeamDTO { TeamID = a.TeamID, Description = a.Description};
        }

        public TicketDTO TicketToDTO(Ticket a)
        {
            return new TicketDTO { Description = a.Description, SubprocessID = a.Subprocess.SubprocessID, StateID = a.State.StateID, TicketID = a.TicketID };
        }
    }
}
