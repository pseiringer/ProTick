 using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProTickDatabase;
using ProTickDatabase.DatabasePOCOs;

namespace ProTick.Controllers
{
    public class UserController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("Employee/{id}")]
        public Employee GetEmployee([FromServices] ProTickDatabaseContext db, int id)
        {
            return db.Employee.First(x => x.EmployeeID == id);
        }

        [HttpGet("Employee")]
        public List<Employee> GetEmployees([FromServices] ProTickDatabaseContext db)
        {
            return db.Employee.ToList();
        }

        [HttpGet("Team/{id}")]
        public Team GetTeam([FromServices] ProTickDatabaseContext db, int id)
        {
            return db.Team.First(x => x.TeamID == id);
        }

        [HttpGet("Team")]
        public List<Team> GetTeams([FromServices] ProTickDatabaseContext db)
        {
            return db.Team.ToList();
        }
    }
}