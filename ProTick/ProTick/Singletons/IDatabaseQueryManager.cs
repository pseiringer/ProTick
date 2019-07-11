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

        EmployeeTeamPrivilege FindEmployeeTeamPrivilegeByID(int id);
        List<EmployeeTeamPrivilege> FindAllEmployeeTeamPrivileges();
        List<EmployeeTeamPrivilege> FindAllEmployeeTeamPrivileges(bool includeReferences);

        ParentChildRelation FindParentChildRelationByID(int id);
        List<ParentChildRelation> FindAllParentChildRelations();
        List<ParentChildRelation> FindAllParentChildRelations(bool includeReferences);

        Privilege FindPrivilegeByID(int id);
        List<Privilege> FindAllPrivileges();
        List<Privilege> FindAllPrivileges(bool includeReferences);

        Process FindProcessByID(int id);
        List<Process> FindAllProcesses();
        List<Process> FindAllProcesses(bool includeReferences);

        State FindStateByID(int id);
        List<State> FindAllStates();
        List<State> FindAllStates(bool includeReferences);

        Subprocess FindSubprocessByID(int id);
        List<Subprocess> FindAllSubprocesses();
        List<Subprocess> FindAllSubprocesses(bool includeReferences);

        Team FindTeamByID(int id);
        List<Team> FindAllTeams();
        List<Team> FindAllTeams(bool includeReferences);

        Ticket FindTicketByID(int id);
        List<Ticket> FindAllTickets();
        List<Ticket> FindAllTickets(bool includeReferences);

    }
}
