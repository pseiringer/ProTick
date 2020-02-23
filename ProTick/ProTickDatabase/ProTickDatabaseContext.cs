using Microsoft.EntityFrameworkCore;
using System;
using ProTickDatabase.DatabasePOCOs;

namespace ProTickDatabase
{
    public class ProTickDatabaseContext : DbContext
    {

        public ProTickDatabaseContext (DbContextOptions opt) : base(opt) { }

        public DbSet<Address> Address { get; set; }
        public DbSet<Employee> Employee { get; set; }
        public DbSet<Team> Team { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<EmployeeTeam> EmployeeTeam { get; set; }
        public DbSet<Process> Process { get; set; }
        public DbSet<Subprocess> Subprocess { get; set; }
        public DbSet<ParentChildRelation> ParentChildRelation { get; set; }
        public DbSet<State> State { get; set; }
        public DbSet<Ticket> Ticket { get; set; }


    }
}


