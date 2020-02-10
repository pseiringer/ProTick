using ProTick.ResourceDTOs;
using ProTickDatabase;
using ProTickDatabase.DatabasePOCOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProTickTest.Classes
{
    public static class DbContextSeeder
    {
        public static ProTickDatabaseContext SeedFull(ProTickDatabaseContext context)
        {
            var addresses = SeedAddresses(context, 3);
            var roles = SeedRoles(context, 3);
            var emps = SeedEmployees(context, 3, addresses, roles);
            var teams = SeedTeams(context, 3);
            var empTeams = SeedEmployeeTeams(context, 3, emps, teams);
            var states = SeedStates(context, 3);
            var processes = SeedProcesses(context, 3);
            var subprocesses = SeedSubprocesses(context, 3, processes, teams);
            var cSubprocesses = new List<Subprocess>() { subprocesses[0], subprocesses[1], subprocesses[2], null };
            var pSubprocesses = new List<Subprocess>() { null, subprocesses[0], subprocesses[1], subprocesses[2] };
            var pcRelations = SeedParentChildRelations(context, 4, cSubprocesses, pSubprocesses);
            var tickets = SeedTickets(context, 3, states, subprocesses);
            context.SaveChanges();
            return context;
        }


        #region SimpleSeeders
        private static List<Address> SeedAddresses(ProTickDatabaseContext context, int amount)
        {
            var addresses = GetSeededAddresses(amount);
            addresses.ForEach(x => context.Add(x));
            context.SaveChanges();
            return addresses;
        }
        private static List<Role> SeedRoles(ProTickDatabaseContext context, int amount)
        {
            var roles = GetSeededRoles(amount);
            roles.ForEach(x => context.Add(x));
            context.SaveChanges();
            return roles;
        }
        private static List<Employee> SeedEmployees(ProTickDatabaseContext context, int amount, List<Address> addresses, List<Role> roles)
        {
            var employees = GetSeededEmployees(amount, addresses, roles);
            employees.ForEach(x => context.Add(x));
            context.SaveChanges();
            return employees;
        }
        private static List<Team> SeedTeams(ProTickDatabaseContext context, int amount)
        {
            var teams = GetSeededTeams(amount);
            teams.ForEach(x => context.Add(x));
            context.SaveChanges();
            return teams;
        }
        private static List<EmployeeTeam> SeedEmployeeTeams(ProTickDatabaseContext context, int amount, List<Employee> emps, List<Team> teams)
        {
            var empTeams = GetSeededEmployeeTeams(amount, emps, teams);
            empTeams.ForEach(x => context.Add(x));
            context.SaveChanges();
            return empTeams;
        }
        private static List<State> SeedStates(ProTickDatabaseContext context, int amount)
        {
            var states = GetSeededStates(amount);
            states.ForEach(x => context.Add(x));
            context.SaveChanges();
            return states;
        }
        private static List<Process> SeedProcesses(ProTickDatabaseContext context, int amount)
        {
            var processes = GetSeededProcesses(amount);
            processes.ForEach(x => context.Add(x));
            context.SaveChanges();
            return processes;
        }
        private static List<Subprocess> SeedSubprocesses(ProTickDatabaseContext context, int amount, List<Process> processes, List<Team> teams)
        {
            var subprocesses = GetSeededSubprocesses(amount, processes, teams);
            subprocesses.ForEach(x => context.Add(x));
            context.SaveChanges();
            return subprocesses;
        }
        private static List<ParentChildRelation> SeedParentChildRelations(ProTickDatabaseContext context, int amount, List<Subprocess> children, List<Subprocess> parents)
        {
            var parentChildRelations = GetSeededParentChildRelations(amount, children, parents);
            parentChildRelations.ForEach(x => context.Add(x));
            context.SaveChanges();
            return parentChildRelations;
        }
        private static List<Ticket> SeedTickets(ProTickDatabaseContext context, int amount, List<State> states, List<Subprocess> subprocesses)
        {
            var tickets = GetSeededTickets(amount, states, subprocesses);
            tickets.ForEach(x => context.Add(x));
            context.SaveChanges();
            return tickets;
        }
        #endregion


        #region GetSeeded
        public static List<Address> GetSeededAddresses(int amount)
        {
            var addresses = new List<Address>();
            for (int i = 0; i < amount; i++)
            {
                addresses.Add(DbPOCOGenerator.GenerateAddress(i + 1));
            }
            
            return addresses;
        }
        public static List<Role> GetSeededRoles(int amount)
        {
            var roles = new List<Role>();
            roles.Add(DbPOCOGenerator.GenerateRole(1, "Employee"));
            roles.Add(DbPOCOGenerator.GenerateRole(2, "Admin"));
            for (int i = 3; i < amount+1; i++)
            {
                roles.Add(DbPOCOGenerator.GenerateRole(i, "Other"+i));
            }

            return roles;
        }
        public static List<Employee> GetSeededEmployees(int amount, List<Address> addresses, List<Role> roles)
        {
            var employees = new List<Employee>();
            for (int i = 0; i < amount; i++)
            {
                employees.Add(DbPOCOGenerator.GenerateEmployee(i + 1, addresses[i], roles[i]));
            }
            
            return employees;
        }
        public static List<Team> GetSeededTeams(int amount)
        {
            var teams = new List<Team>();
            for (int i = 0; i < amount; i++)
            {
                teams.Add(DbPOCOGenerator.GenerateTeam(i + 1));
            }
            
            return teams;
        }
        public static List<EmployeeTeam> GetSeededEmployeeTeams(int amount, List<Employee> emps, List<Team> teams)
        {
            var empTeams = new List<EmployeeTeam>();
            for (int i = 0; i < amount; i++)
            {
                empTeams.Add(DbPOCOGenerator.GenerateEmployeeTeam(i + 1, emps[i], teams[i]));
            }
            
            return empTeams;
        }
        public static List<State> GetSeededStates(int amount)
        {
            var states = new List<State>();
            for (int i = 0; i < amount; i++)
            {
                states.Add(DbPOCOGenerator.GenerateState(i + 1));
            }
            
            return states;
        }
        public static List<Process> GetSeededProcesses(int amount)
        {
            var processes = new List<Process>();
            for (int i = 0; i < amount; i++)
            {
                processes.Add(DbPOCOGenerator.GenerateProcess(i + 1));
            }
            
            return processes;
        }
        public static List<Subprocess> GetSeededSubprocesses(int amount, List<Process> processes, List<Team> teams)
        {
            var subprocesses = new List<Subprocess>();
            for (int i = 0; i < amount; i++)
            {
                subprocesses.Add(DbPOCOGenerator.GenerateSubprocess(i + 1, processes[i], teams[i]));
            }
            
            return subprocesses;
        }
        public static List<ParentChildRelation> GetSeededParentChildRelations(int amount, List<Subprocess> children, List<Subprocess> parents)
        {
            var parentChildRelations = new List<ParentChildRelation>();
            for (int i = 0; i < amount; i++)
            {
                parentChildRelations.Add(DbPOCOGenerator.GenerateParentChildRelation(i + 1, children[i], parents[i]));
            }
            
            return parentChildRelations;
        }
        public static List<Ticket> GetSeededTickets(int amount, List<State> states, List<Subprocess> subprocesses)
        {
            var tickets = new List<Ticket>();
            for (int i = 0; i < amount; i++)
            {
                tickets.Add(DbPOCOGenerator.GenerateTicket(i + 1, states[i], subprocesses[i]));
            }
            
            return tickets;
        }
        #endregion


        #region GetSeededDTOs
        public static List<AddressDTO> GetSeededAddressDTOs(int amount)
        {
            var addresses = new List<AddressDTO>();
            for (int i = 0; i < amount; i++)
            {
                addresses.Add(DbPOCOGenerator.GenerateAddressDTO(i + 1));
            }
            return addresses;
        }
        public static List<RoleDTO> GetSeededRoleDTOs(int amount)
        {
            var roles = new List<RoleDTO>();

            roles.Add(DbPOCOGenerator.GenerateRoleDTO(1, "Employee"));
            roles.Add(DbPOCOGenerator.GenerateRoleDTO(2, "Admin"));
            for (int i = 3; i < amount + 1; i++)
            {
                roles.Add(DbPOCOGenerator.GenerateRoleDTO(3, "Other"+i));
            }

            return roles;
        }
        public static List<EmployeeDTO> GetSeededEmployeeDTOs(int amount, List<AddressDTO> addresses, List<RoleDTO> roles)
        {
            var employeeDTOs = new List<EmployeeDTO>();
            for (int i = 0; i < amount; i++)
            {
                employeeDTOs.Add(DbPOCOGenerator.GenerateEmployeeDTO(i + 1, addresses[i].AddressID, roles[i].RoleID));
            }
            
            return employeeDTOs;
        }
        public static List<TeamDTO> GetSeededTeamDTOs(int amount)
        {
            var teamDTOs = new List<TeamDTO>();
            for (int i = 0; i < amount; i++)
            {
                teamDTOs.Add(DbPOCOGenerator.GenerateTeamDTO(i + 1));
            }
            
            return teamDTOs;
        }
        public static List<EmployeeTeamDTO> GetSeededEmployeeTeamDTOs(int amount, List<EmployeeDTO> emps, List<TeamDTO> teams)
        {
            var empTeamDTOs = new List<EmployeeTeamDTO>();
            for (int i = 0; i < amount; i++)
            {
                empTeamDTOs.Add(DbPOCOGenerator.GenerateEmployeeTeamDTO(i + 1, emps[i].EmployeeID, teams[i].TeamID));
            }
            
            return empTeamDTOs;
        }
        public static List<StateDTO> GetSeededStateDTOs(int amount)
        {
            var stateDTOs = new List<StateDTO>();
            for (int i = 0; i < amount; i++)
            {
                stateDTOs.Add(DbPOCOGenerator.GenerateStateDTO(i + 1));
            }
            
            return stateDTOs;
        }
        public static List<ProcessDTO> GetSeededProcessDTOs(int amount)
        {
            var processDTOs = new List<ProcessDTO>();
            for (int i = 0; i < amount; i++)
            {
                processDTOs.Add(DbPOCOGenerator.GenerateProcessDTO(i + 1));
            }
            
            return processDTOs;
        }
        public static List<SubprocessDTO> GetSeededSubprocessDTOs(int amount, List<ProcessDTO> processes, List<TeamDTO> teams)
        {
            var subprocessDTOs = new List<SubprocessDTO>();
            for (int i = 0; i < amount; i++)
            {
                subprocessDTOs.Add(DbPOCOGenerator.GenerateSubprocessDTO(i + 1, processes[i].ProcessID, teams[i].TeamID));
            }
            
            return subprocessDTOs;
        }
        public static List<ParentChildRelationDTO> GetSeededParentChildRelationDTOs(int amount, List<SubprocessDTO> children, List<SubprocessDTO> parents)
        {
            var parentChildRelationDTOs = new List<ParentChildRelationDTO>();
            for (int i = 0; i < amount; i++)
            {
                parentChildRelationDTOs.Add(DbPOCOGenerator.GenerateParentChildRelationDTO(
                    i + 1,
                    (children[i] != null) ? children[i].SubprocessID : -1,
                    (parents[i] != null) ? parents[i].SubprocessID : -1));
            }
            
            return parentChildRelationDTOs;
        }
        public static List<TicketDTO> GetSeededTicketDTOs(int amount, List<StateDTO> states, List<SubprocessDTO> subprocesses)
        {
            var ticketDTOs = new List<TicketDTO>();
            for (int i = 0; i < amount; i++)
            {
                ticketDTOs.Add(DbPOCOGenerator.GenerateTicketDTO(i + 1, states[i].StateID, subprocesses[i].SubprocessID));
            }
            
            return ticketDTOs;
        } 
        #endregion
    }
}
