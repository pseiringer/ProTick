using ProTick.ResourceDTOs;
using ProTickDatabase.DatabasePOCOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProTickTest.Classes
{
    public static class DbPOCOGenerator
    {
        #region POCOs

        public static Address GenerateAddress(int id)
        {
            return new Address
            {
                AddressID = id,
                City = "city" + id,
                Country = "country" + id,
                PostalCode = "zip" + id,
                Street = "street" + id,
                StreetNumber = "streetNr" + id
            };
        }
        public static Employee GenerateEmployee(int id, Address address, Role role)
        {
            return new Employee
            {
                EmployeeID = id,
                FirstName = "firstName" + id,
                LastName = "lastName" + id,
                Username = "a" + id,
                Password = "",
                DateOfBirth = DateTime.Parse("01/01/2020"),
                HireDate = DateTime.Parse("01/01/2020"),
                Email = "mail" + id,
                PhoneNumber = "phone" + id,
                Address = address,
                Role = role
            };
        }
        public static EmployeeTeam GenerateEmployeeTeam(int id, Employee employee, Team team)
        {
            return new EmployeeTeam
            {
                EmployeeTeamID = id,
                Employee = employee,
                Team = team
            };
        }
        public static ParentChildRelation GenerateParentChildRelation(int id, Subprocess child, Subprocess parent)
        {
            return new ParentChildRelation
            {
                ParentChildRelationID = id,
                Child = child,
                Parent = parent
            };
        }
        public static Process GenerateProcess(int id)
        {
            return new Process
            {
                ProcessID = id,
                Description = "desc" + id
            };
        }
        public static Role GenerateRole(int id)
        {
            return GenerateRole(id, "title" + id);
        }
        public static Role GenerateRole(int id, string title)
        {
            return new Role
            {
                RoleID = id,
                Title = title
            };
        }
        public static State GenerateState(int id)
        {
            return new State
            {
                StateID = id,
                Description = "desc" + id
            };
        }
        public static Subprocess GenerateSubprocess(int id, Process process, Team team)
        {
            return new Subprocess
            {
                SubprocessID = id,
                Description = "desc" + id,
                Process = process,
                Team = team
            };
        }
        public static Team GenerateTeam(int id)
        {
            return new Team
            {
                TeamID = id,
                Description = "desc" + id,
                Abbreviation = "abbreviation" + id
            };
        }
        public static Ticket GenerateTicket(int id, State state, Subprocess subprocess)
        {
            return new Ticket
            {
                TicketID = id,
                Description = "a" + id,
                Note = "a" + id,
                State = state,
                Subprocess = subprocess
            };
        }

        #endregion

        #region DTOs
        public static AddressDTO GenerateAddressDTO(int id)
        {
            return new AddressDTO
            {
                AddressID = id,
                City = "city" + id,
                Country = "country" + id,
                PostalCode = "zip" + id,
                Street = "street" + id,
                StreetNumber = "streetNr" + id
            };
        }
        public static EmployeeDTO GenerateEmployeeDTO(int id, int addressId, int roleId)
        {
            return new EmployeeDTO
            {
                EmployeeID = id,
                FirstName = "firstName" + id,
                LastName = "lastName" + id,
                Username = "a" + id,
                DateOfBirth = DateTime.Parse("01/01/2020").ToShortDateString(),
                HireDate = DateTime.Parse("01/01/2020").ToShortDateString(),
                Email = "mail" + id,
                PhoneNumber = "phone" + id,
                AddressID = addressId,
                RoleID = roleId
            };
        }
        public static EmployeeTeamDTO GenerateEmployeeTeamDTO(int id, int employeeId, int teamId)
        {
            return new EmployeeTeamDTO
            {
                EmployeeTeamID = id,
                EmployeeID = employeeId,
                TeamID = teamId
            };
        }
        public static ParentChildRelationDTO GenerateParentChildRelationDTO(int id, int childId, int parentId)
        {
            return new ParentChildRelationDTO
            {
                ParentChildRelationID = id,
                ChildID = childId,
                ParentID = parentId
            };
        }
        public static ProcessDTO GenerateProcessDTO(int id)
        {
            return new ProcessDTO
            {
                ProcessID = id,
                Description = "desc" + id
            };
        }
        public static RoleDTO GenerateRoleDTO(int id)
        {
            return GenerateRoleDTO(id, "title" + id);
        }
        public static RoleDTO GenerateRoleDTO(int id, string title)
        {
            return new RoleDTO
            {
                RoleID = id,
                Title = title
            };
        }
        public static StateDTO GenerateStateDTO(int id)
        {
            return new StateDTO
            {
                StateID = id,
                Description = "desc" + id
            };
        }
        public static SubprocessDTO GenerateSubprocessDTO(int id, int processId, int teamId)
        {
            return new SubprocessDTO
            {
                SubprocessID = id,
                Description = "desc" + id,
                ProcessID = processId,
                TeamID = teamId
            };
        }
        public static TeamDTO GenerateTeamDTO(int id)
        {
            return new TeamDTO
            {
                TeamID = id,
                Description = "desc" + id,
                Abbreviation = "abbreviation" + id
            };
        }
        public static TicketDTO GenerateTicketDTO(int id, int stateId, int subprocessId)
        {
            return new TicketDTO
            {
                TicketID = id,
                Description = "a" + id,
                Note = "a" + id,
                StateID = stateId,
                SubprocessID = subprocessId
            };
        } 
        #endregion
    }
}
