﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProTick.Exceptions;
using ProTickDatabase;
using ProTickDatabase.DatabasePOCOs;

namespace ProTick.Singletons
{
    public class DatabaseQueryManager : IDatabaseQueryManager
    {
        private readonly ProTickDatabaseContext db;

        public DatabaseQueryManager([FromServices] ProTickDatabaseContext db)
        {
            this.db = db;
        }


        #region --- FindByID ---

        public Address FindAddressByID(int id)
        {
            var address = db.Address.FirstOrDefault(x => x.AddressID == id);
            if (address == null) throw new DatabaseEntryNotFoundException($"Address with ID ({id}) was not found");
            return address;
        }

        public Employee FindEmployeeByID(int id)
        {
            var employee = db.Employee.FirstOrDefault(x => x.EmployeeID == id);
            if (employee == null) throw new DatabaseEntryNotFoundException($"Employee with ID ({id}) was not found");
            return employee;
        }

        public EmployeeTeam FindEmployeeTeamByID(int id)
        {
            var employeeTeam = db.EmployeeTeam.FirstOrDefault(x => x.EmployeeTeamID == id);
            if (employeeTeam == null) throw new DatabaseEntryNotFoundException($"EmployeeTeam with ID ({id}) was not found");
            return employeeTeam;
        }

        public EmployeeTeamPrivilege FindEmployeeTeamPrivilegeByID(int id)
        {
            var employeeTeamPrivilege = db.EmployeeTeamPrivilege.FirstOrDefault(x => x.EmployeeTeamPrivilegeID == id);
            if (employeeTeamPrivilege == null) throw new DatabaseEntryNotFoundException($"EmployeeTeamPrivilege with ID ({id}) was not found");
            return employeeTeamPrivilege;
        }

        public ParentChildRelation FindParentChildRelationByID(int id)
        {
            var parentChildRelation = db.ParentChildRelation.FirstOrDefault(x => x.ParentChildRelationID == id);
            if (parentChildRelation == null) throw new DatabaseEntryNotFoundException($"ParentChildRelation with ID ({id}) was not found");
            return parentChildRelation;
        }

        public Privilege FindPrivilegeByID(int id)
        {
            var privilege = db.Privilege.FirstOrDefault(x => x.PrivilegeID == id);
            if (privilege == null) throw new DatabaseEntryNotFoundException($"Privilege with ID ({id}) was not found");
            return privilege;
        }

        public Process FindProcessByID(int id)
        {
            var process = db.Process.FirstOrDefault(x => x.ProcessID == id);
            if (process == null) throw new DatabaseEntryNotFoundException($"Process with ID ({id}) was not found");
            return process;
        }

        public State FindStateByID(int id)
        {
            var state = db.State.FirstOrDefault(x => x.StateID == id);
            if (state == null) throw new DatabaseEntryNotFoundException($"State with ID ({id}) was not found");
            return state;
        }

        public Subprocess FindSubprocessByID(int id)
        {
            var subprocess = db.Subprocess.FirstOrDefault(x => x.SubprocessID == id);
            if (subprocess == null) throw new DatabaseEntryNotFoundException($"Subprocess with ID ({id}) was not found");
            return subprocess;
        }

        public Team FindTeamByID(int id)
        {
            var team = db.Team.FirstOrDefault(x => x.TeamID == id);
            if (team == null) throw new DatabaseEntryNotFoundException($"Team with ID ({id}) was not found");
            return team;
        }

        public Ticket FindTicketByID(int id)
        {
            var ticket = db.Ticket.Include(x => x.Subprocess).Include(x => x.State).FirstOrDefault(x => x.TicketID == id);
            if (ticket == null) throw new DatabaseEntryNotFoundException($"Ticket with ID ({id}) was not found");
            return ticket;
        }


        #endregion


        #region --- FindAll ---

        public List<Address> FindAllAddresses()
        {
            return FindAllAddresses(false);
        }

        public List<Employee> FindAllEmployees()
        {
            return FindAllEmployees(false);
        }

        public List<EmployeeTeamPrivilege> FindAllEmployeeTeamPrivileges()
        {
            return FindAllEmployeeTeamPrivileges(false);
        }

        public List<EmployeeTeam> FindAllEmployeeTeams()
        {
            return FindAllEmployeeTeams(false);
        }

        public List<ParentChildRelation> FindAllParentChildRelations()
        {
            return FindAllParentChildRelations(false);
        }

        public List<Privilege> FindAllPrivileges()
        {
            return FindAllPrivileges(false);
        }

        public List<Process> FindAllProcesses()
        {
            return FindAllProcesses(false);
        }

        public List<State> FindAllStates()
        {
            return FindAllStates(false);
        }

        public List<Subprocess> FindAllSubprocesses()
        {
            return FindAllSubprocesses(false);
        }

        public List<Team> FindAllTeams()
        {
            return FindAllTeams(false);
        }

        public List<Ticket> FindAllTickets()
        {
            return FindAllTickets(false);
        }

        #endregion


        #region --- FindAll (bool includeReferences) ---

        public List<Address> FindAllAddresses(bool includeReferences)
        {
            return db.Address.ToList();
        }

        public List<Employee> FindAllEmployees(bool includeReferences)
        {
            if (includeReferences) return db.Employee.Include(x => x.Address).ToList();
            return db.Employee.ToList();
        }

        public List<EmployeeTeamPrivilege> FindAllEmployeeTeamPrivileges(bool includeReferences)
        {
            if (includeReferences) return db.EmployeeTeamPrivilege.Include(x => x.Privilege).Include(x => x.EmployeeTeam).ToList();
            return db.EmployeeTeamPrivilege.ToList();
        }

        public List<EmployeeTeam> FindAllEmployeeTeams(bool includeReferences)
        {
            if (includeReferences) return db.EmployeeTeam.Include(x => x.Employee).Include(x => x.Team).ToList();
            return db.EmployeeTeam.ToList();
        }

        public List<ParentChildRelation> FindAllParentChildRelations(bool includeReferences)
        {
            if (includeReferences) return db.ParentChildRelation.Include(x => x.Child).Include(x => x.Parent).ToList();
            return db.ParentChildRelation.ToList();
        }

        public List<Privilege> FindAllPrivileges(bool includeReferences)
        {
            return db.Privilege.ToList();
        }

        public List<Process> FindAllProcesses(bool includeReferences)
        {
            return db.Process.ToList();
        }

        public List<State> FindAllStates(bool includeReferences)
        {
            return db.State.ToList();
        }

        public List<Subprocess> FindAllSubprocesses(bool includeReferences)
        {
            if (includeReferences) return db.Subprocess.Include(x => x.Process).Include(x => x.Team).ToList();
            return db.Subprocess.ToList();
        }

        public List<Team> FindAllTeams(bool includeReferences)
        {
            return db.Team.ToList();
        }

        public List<Ticket> FindAllTickets(bool includeReferences)
        {
            if (includeReferences) return db.Ticket.Include(x => x.State).Include(x => x.Subprocess).ToList();
            return db.Ticket.ToList();
        }

        #endregion

    }
}