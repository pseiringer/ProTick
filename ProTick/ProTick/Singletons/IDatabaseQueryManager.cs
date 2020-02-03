using ProTickDatabase.DatabasePOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProTick.Singletons
{
    public interface IDatabaseQueryManager
    {
        Address FindAddressByID(int id);
        List<Address> FindAllAddresses();
        List<Address> FindAllAddresses(bool includeReferences);

        Employee FindEmployeeByID(int id);
        List<Employee> FindAllEmployees();
        List<Employee> FindAllEmployees(bool includeReferences);
        
        EmployeeTeam FindEmployeeTeamByID(int id);
        List<EmployeeTeam> FindAllEmployeeTeams();
        List<EmployeeTeam> FindAllEmployeeTeams(bool includeReferences);
        List<EmployeeTeam> FindEmployeeTeamsByEmployeeID(int id);
        List<EmployeeTeam> FindEmployeeTeamsByTeamID(int id);
        ParentChildRelation FindParentChildRelationByID(int id);
        List<ParentChildRelation> FindAllParentChildRelations();
        List<ParentChildRelation> FindAllParentChildRelations(bool includeReferences);
        List<ParentChildRelation> FindAllParentChildRelationsOfProcess(int ProcessID);

        Role FindRoleByID(int id);
        List<Role> FindAllRoles();
        List<Role> FindAllRoles(bool includeReferences);

        Process FindProcessByID(int id);
        List<Process> FindAllProcesses();
        List<Process> FindAllProcesses(bool includeReferences);
        List<Process> FindAllProcessesWithSubprocess(bool hasSubprocess);
        State FindStateByID(int id);
        List<State> FindAllStates();
        List<State> FindAllStates(bool includeReferences);
        Subprocess FindSubprocessByID(int id);
        List<Subprocess> FindAllSubprocesses();
        List<Subprocess> FindAllSubprocesses(bool includeReferences);
        List<Ticket> FindAllTicketsBySubprocessID(int id);
        List<Subprocess> FindAllChildrenBySubprocessID(int id);

        Team FindTeamByID(int id);
        List<Team> FindAllTeams();
        List<Team> FindAllTeams(bool includeReferences);
        List<Team> FindAllTeamsByUsername(string user);

        Ticket FindTicketByID(int id);
        List<Ticket> FindAllTickets();
        List<Ticket> FindAllTickets(bool includeReferences);
        List<Ticket> FindAllTicketsByTeamID(int id);
        List<Ticket> FindAllTicketsByStateID(int id);
        List<Ticket> FindAllTicketsByUsername(string username);


        Employee FindEmployeeByUsername(string username);

    }
}
