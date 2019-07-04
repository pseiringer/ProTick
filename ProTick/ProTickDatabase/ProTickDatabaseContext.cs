using Microsoft.EntityFrameworkCore;
using System;
using ProTickDatabase.DatabasePOCOs;

namespace ProTickDatabase
{
    public class ProTickDatabaseContext : DbContext
    {

        public ProTickDatabaseContext (DbContextOptions opt) : base(opt) { }

        public DbSet<Address> Addresses { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Privilege> Privileges { get; set; }
        public DbSet<EmployeeTeam> EmployeeTeams { get; set; }
        public DbSet<EmployeeTeamPrivilege> EmployeeTeamPrivileges { get; set; }
        public DbSet<Process> Processes { get; set; }
        public DbSet<Subprocess> Subprocesses { get; set; }
        public DbSet<ParentChildRelation> ParentChildRelation { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<Ticket> Tickets { get; set; }


    }
}
