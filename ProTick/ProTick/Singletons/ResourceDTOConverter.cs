using ProTick.ResourceDTOs;
using ProTickDatabase.DatabasePOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProTickDatabase;
using Microsoft.AspNetCore.Mvc;

namespace ProTick.Singletons
{
    public class ResourceDTOConverter : IResourceDTOConverter
    {
        private readonly IDatabaseQueryManager dbm;

        public ResourceDTOConverter([FromServices] IDatabaseQueryManager dbm)
        {
            this.dbm = dbm;
        }
        

        #region ------ DB to DTO
        public AddressDTO AddressToDTO(Address a)
        {
            return new AddressDTO { AddressID = a.AddressID, Country = a.Country, City = a.City, PostalCode = a.PostalCode, Street = a.Street, StreetNumber = a.StreetNumber };
        }

        public EmployeeDTO EmployeeToDTO(Employee a)
        {
            return new EmployeeDTO { AddressID = a.Address.AddressID, DateOfBirth = a.DateOfBirth.ToShortDateString(), EmployeeID = a.EmployeeID, FirstName = a.FirstName, HireDate = a.HireDate.ToShortDateString(), LastName = a.LastName, Username = a.Username, Password = a.Password};
        }

        public EmployeeTeamDTO EmployeeTeamToDTO(EmployeeTeam a)
        {
            return new EmployeeTeamDTO { EmployeeID = a.Employee.EmployeeID, EmployeeTeamID = a.EmployeeTeamID, RoleID = a.Role.RoleID, TeamID = a.Team.TeamID };
        }

        public ParentChildRelationDTO ParentChildRelationToDTO(ParentChildRelation a)
        {
            return new ParentChildRelationDTO { ChildID = a.Child.SubprocessID, ParentChildRelationID = a.ParentChildRelationID, ParentID = a.Parent.SubprocessID };
        }

        public RoleDTO PrivilegeToDTO(Role a)
        {
            return new RoleDTO { RoleID = a.RoleID, Title = a.Title };
        }

        public ProcessDTO ProcessToDTO(Process a)
        {
            return new ProcessDTO { Description = a.Description, ProcessID = a.ProcessID };
        }

        public StateDTO StateToDTO(State a)
        {
            return new StateDTO { Description = a.Description, StateID = a.StateID };
        }

        public SubprocessDTO SubprocessToDTO(Subprocess a)
        {
            return new SubprocessDTO { Description = a.Description, ProcessID = a.Process.ProcessID, SubprocessID = a.SubprocessID, TeamID = a.Team.TeamID };
        }

        public TeamDTO TeamToDTO(Team a)
        {
            return new TeamDTO { TeamID = a.TeamID, Description = a.Description, Abbreviation = a.Abbreviation };
        }

        public TicketDTO TicketToDTO(Ticket a)
        {
            return new TicketDTO { Description = a.Description, SubprocessID = a.Subprocess.SubprocessID, StateID = a.State.StateID, TicketID = a.TicketID };
        }

        #endregion

        #region ------ DTO to DB

        public Address DTOToAddress(AddressDTO a)
        {
            return new Address { AddressID = a.AddressID, Country = a.Country, City = a.City, PostalCode = a.PostalCode, Street = a.Street, StreetNumber = a.StreetNumber };
        }

        public Employee DTOToEmployee(EmployeeDTO a)
        {            
            return new Employee { Address = dbm.FindAddressByID(a.AddressID), DateOfBirth = DateTime.Parse(a.DateOfBirth), EmployeeID = a.EmployeeID, FirstName = a.FirstName, HireDate = DateTime.Parse(a.HireDate), Password = a.Password, Username = a.Username, LastName = a.LastName };
        }

        public EmployeeTeam DTOToEmployeeTeam(EmployeeTeamDTO a)
        {
            return new EmployeeTeam { Employee = dbm.FindEmployeeByID(a.EmployeeID), EmployeeTeamID = a.EmployeeTeamID, Role = dbm.FindRoleByID(a.RoleID), Team = dbm.FindTeamByID(a.TeamID)};
        }
        
        public ParentChildRelation DTOToParentChildRelation(ParentChildRelationDTO a)
        {
            return new ParentChildRelation { Child = dbm.FindSubprocessByID(a.ChildID), ParentChildRelationID = a.ParentChildRelationID, Parent = dbm.FindSubprocessByID(a.ParentID) };
        }

        public Role DTOToPrivilege(RoleDTO a)
        {
            return new Role { RoleID = a.RoleID, Title = a.Title };
        }

        public Process DTOToProcess(ProcessDTO a)
        {
            return new Process { Description = a.Description, ProcessID = a.ProcessID };
        }

        public State DTOToState(StateDTO a)
        {
            return new State { Description = a.Description, StateID = a.StateID };
        }

        public Subprocess DTOToSubprocess(SubprocessDTO a)
        {
            return new Subprocess { Description = a.Description, Process = dbm.FindProcessByID(a.ProcessID), SubprocessID = a.SubprocessID, Team = dbm.FindTeamByID(a.TeamID) };
        }

        public Team DTOToTeam(TeamDTO a)
        {
            return new Team { TeamID = a.TeamID, Description = a.Description, Abbreviation = a.Abbreviation };
        }

        public Ticket DTOToTicket(TicketDTO a)
        {
            return new Ticket { Description = a.Description, Subprocess = dbm.FindSubprocessByID(a.SubprocessID), State = dbm.FindStateByID(a.StateID), TicketID = a.TicketID };
        }


        #endregion
    }
}
