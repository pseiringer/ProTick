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
            if(a.Address == null)
            return new EmployeeDTO { AddressID = 0, RoleID = a.Role.RoleID, DateOfBirth = (a.DateOfBirth.HasValue) ? a.DateOfBirth.Value.ToShortDateString() : "", EmployeeID = a.EmployeeID, FirstName = a.FirstName, Email = a.Email, PhoneNumber = a.PhoneNumber, HireDate = (a.HireDate.HasValue) ? a.HireDate.Value.ToShortDateString() : "", LastName = a.LastName, Username = a.Username };
            else
            return new EmployeeDTO { AddressID = a.Address.AddressID, DateOfBirth = (a.DateOfBirth.HasValue) ? a.DateOfBirth.Value.ToShortDateString() : "", EmployeeID = a.EmployeeID, FirstName = a.FirstName, Email = a.Email, PhoneNumber = a.PhoneNumber, HireDate = (a.HireDate.HasValue) ? a.HireDate.Value.ToShortDateString() : "", LastName = a.LastName, Username = a.Username, RoleID = a.Role.RoleID };
        }

        public EmployeeTeamDTO EmployeeTeamToDTO(EmployeeTeam a)
        {
            return new EmployeeTeamDTO { EmployeeID = a.Employee.EmployeeID, EmployeeTeamID = a.EmployeeTeamID, TeamID = a.Team.TeamID };
        }

        public RoleDTO RoleToDTO(Role a)
        {
            return new RoleDTO { RoleID = a.RoleID, Title = a.Title };
        }

        public ParentChildRelationDTO ParentChildRelationToDTO(ParentChildRelation a)
        {
            int childID = -1;
            if (a.Child != null) childID = a.Child.SubprocessID;
            int parentID = -1;
            if (a.Parent != null) parentID = a.Parent.SubprocessID;

            return new ParentChildRelationDTO { ChildID = childID, ParentChildRelationID = a.ParentChildRelationID, ParentID = parentID };
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
            int subprocessID = -1;
            if (a.Subprocess != null) subprocessID = a.Subprocess.SubprocessID;

            return new TicketDTO { Description = a.Description, Note = a.Note, SubprocessID = subprocessID, StateID = a.State.StateID, TicketID = a.TicketID };
        }

        #endregion

        #region ------ DTO to DB

        public Address DTOToAddress(AddressDTO a)
        {
            return new Address { AddressID = a.AddressID, Country = a.Country, City = a.City, PostalCode = a.PostalCode, Street = a.Street, StreetNumber = a.StreetNumber };
        }

        public Employee DTOToEmployee(EmployeeDTO a)
        {
            Console.WriteLine("-------------" + a.RoleID);
            if (a.AddressID <= 0)
                return new Employee { Address = null, Role = dbm.FindRoleByID(a.RoleID), DateOfBirth = DateTime.Parse(a.DateOfBirth), PhoneNumber = a.PhoneNumber, Email = a.Email, EmployeeID = a.EmployeeID, FirstName = a.FirstName, HireDate = DateTime.Parse(a.HireDate), Username = a.Username, LastName = a.LastName };
            else
                return new Employee { Address = dbm.FindAddressByID(a.AddressID), DateOfBirth = DateTime.Parse(a.DateOfBirth), PhoneNumber = a.PhoneNumber, Email = a.Email, EmployeeID = a.EmployeeID, FirstName = a.FirstName, HireDate = DateTime.Parse(a.HireDate), Username = a.Username, LastName = a.LastName, Role = dbm.FindRoleByID(a.RoleID) };
        }

        public EmployeeTeam DTOToEmployeeTeam(EmployeeTeamDTO a)
        {
            return new EmployeeTeam { Employee = dbm.FindEmployeeByID(a.EmployeeID), EmployeeTeamID = a.EmployeeTeamID, Team = dbm.FindTeamByID(a.TeamID)};
        }

        public Role DTOToRole(RoleDTO a)
        {
            return new Role { Title = a.Title, RoleID = a.RoleID };
        }

        public ParentChildRelation DTOToParentChildRelation(ParentChildRelationDTO a)
        {
            return new ParentChildRelation { Child = dbm.FindSubprocessByID(a.ChildID), ParentChildRelationID = a.ParentChildRelationID, Parent = dbm.FindSubprocessByID(a.ParentID) };
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
            return new Ticket { Description = a.Description, Note = a.Note, Subprocess = dbm.FindSubprocessByID(a.SubprocessID), State = dbm.FindStateByID(a.StateID), TicketID = a.TicketID };
        }


        #endregion
    }
}
