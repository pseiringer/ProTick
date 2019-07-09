using ProTick.ResourceDTOs;
using ProTickDatabase.DatabasePOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProTickDatabase;


namespace ProTick
{
    public class ResourceDTOConverter
    {
        ProTickDatabaseContext db;

        public ResourceDTOConverter(ProTickDatabaseContext db)
        {
            this.db = db;
        }

        public ResourceDTOConverter()
        {
        }



        #region ------ DB to DTO
        public AddressDTO AddressToDTO(Address a)
        {
            return new AddressDTO { AddressID = a.AddressID, Country = a.Country, City = a.City, PostalCode = a.PostalCode, Street = a.Street, StreetNumber = a.StreetNumber };
        }

        public EmployeeDTO EmployeeToDTO(Employee a)
        {
            return new EmployeeDTO { AddressID = a.Address.AddressID, DateOfBirth = a.DateOfBirth, EmployeeID = a.EmployeeID, FirstName = a.FirstName, HireDate = a.HireDate, LastName = a.LastName, Username = a.Username, Password = a.Password};
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
            return new ParentChildRelationDTO { ChildID = a.Child.SubprocessID, ParentChildRelationID = a.ParentChildRelationID, ParentID = a.Parent.SubprocessID };
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
            return new StateDTO { Description = a.Description, StateID = a.StateID };
        }

        public SubprocessDTO SubprocessToDTO(Subprocess a)
        {
            return new SubprocessDTO { Description = a.Description, ProcessID = a.Process.ProcessID, SubprocessID = a.SubprocessID, TeamID = a.Team.TeamID };
        }

        public TeamDTO TeamToDTO(Team a)
        {
            return new TeamDTO { TeamID = a.TeamID, Description = a.Description };
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
            if (db == null)
            {
                return new Employee { Address = null, DateOfBirth = a.DateOfBirth, EmployeeID = a.EmployeeID, FirstName = a.FirstName, HireDate = a.HireDate, Password = a.Password, Username = a.Username, LastName = a.LastName };
            }
            else
            {
                return new Employee { Address = db.Address.First(x => x.AddressID == a.AddressID), DateOfBirth = a.DateOfBirth, EmployeeID = a.EmployeeID, FirstName = a.FirstName, HireDate = a.HireDate, Password = a.Password, Username = a.Username, LastName = a.LastName };
            }
        }

        public EmployeeTeam DTOToEmployeeTeam(EmployeeTeamDTO a)
        {
            if (db == null)
            {
                return new EmployeeTeam { Employee = null, EmployeeTeamID = a.EmployeeTeamID, Role = a.Role, Team = null };

            }
            else
            {
                return new EmployeeTeam { Employee = db.Employee.First(x => x.EmployeeID == a.EmployeeID), EmployeeTeamID = a.EmployeeTeamID, Role = a.Role, Team = db.Team.First(x => x.TeamID == a.TeamID) };

            }
        }

        public EmployeeTeamPrivilege DTOToEmployeeTeamPrivilege(EmployeeTeamPrivilegeDTO a)
        {
            if (db == null)
            {
                return new EmployeeTeamPrivilege { EmployeeTeam = null, EmployeeTeamPrivilegeID = a.EmployeeTeamPrivilegeID, Privilege = null };

            }
            else
            {
                return new EmployeeTeamPrivilege { EmployeeTeam = db.EmployeeTeam.First(x => x.EmployeeTeamID == a.EmployeeTeamID), EmployeeTeamPrivilegeID = a.EmployeeTeamPrivilegeID, Privilege = db.Privilege.First(x => x.PrivilegeID == a.PrivilegeID)};
<<<<<<< HEAD
=======

>>>>>>> 5666fbdd57680687a60afaae176b9e9d3ed19a18
            }
        }

        public ParentChildRelation DTOToParentChildRelation(ParentChildRelationDTO a)
        {
            if (db == null)
            {
                return new ParentChildRelation { Child = null, ParentChildRelationID = a.ParentChildRelationID, Parent = null };

            }
            else
            {
                return new ParentChildRelation { Child = db.Subprocess.First(x => x.SubprocessID == a.ChildID), ParentChildRelationID = a.ParentChildRelationID, Parent = db.Subprocess.First(x => x.SubprocessID == a.ParentID) };

            }
        }

        public Privilege DTOToPrivilege(PrivilegeDTO a)
        {
            return new Privilege { PrivilegeID = a.PrivilegeID, Description = a.Description };
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
            if (db == null)
            {
                return new Subprocess { Description = a.Description, Process = null, SubprocessID = a.SubprocessID, Team = null };

            }
            else
            {
                return new Subprocess { Description = a.Description, Process = db.Process.First(x => x.ProcessID == a.ProcessID), SubprocessID = a.SubprocessID, Team = db.Team.First(x => x.TeamID == a.TeamID)};

            }
        }

        public Team DTOToTeam(TeamDTO a)
        {
            return new Team { TeamID = a.TeamID, Description = a.Description };
        }

        public Ticket DTOToTicket(TicketDTO a)
        {
            if (db == null)
            {
                return new Ticket { Description = a.Description, Subprocess = null, State = null, TicketID = a.TicketID };

            }
            else
            {
                return new Ticket { Description = a.Description, Subprocess = db.Subprocess.First(x => x.SubprocessID == a.SubprocessID), State = db.State.First(x => x.StateID == a.StateID), TicketID = a.TicketID };

            }
        }


        #endregion
    }
}
