using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProTick.ResourceDTOs;
using ProTick.Singletons;
using ProTickDatabase;
using ProTickDatabase.DatabasePOCOs;

namespace ProTick.Controllers
{
    [Route("ProTick/[controller]")]
    public class EmployeeTeamController : Controller
    {
        private ProTickDatabaseContext db;
        private IResourceDTOConverter converter;
        private IDatabaseQueryManager dbm;

        public EmployeeTeamController([FromServices] ProTickDatabaseContext db, [FromServices] IResourceDTOConverter converter, [FromServices] IDatabaseQueryManager dbm)
        {
            this.db = db;
            this.converter = converter;
            this.dbm = dbm;
        }

        [HttpGet("{id}")]
        public EmployeeTeamDTO GetEmployeeTeam(int id)
        {
            return converter.EmployeeTeamToDTO(dbm.FindEmployeeTeamByID(id));

        }

        [HttpGet]
        public IEnumerable<EmployeeTeamDTO> GetEmployeeTeams()
        {
            return dbm.FindAllEmployeeTeams(true).Select(x => converter.EmployeeTeamToDTO(x)).ToList();
        }


        [HttpPost]
        public EmployeeTeamDTO NewEmployeeTeam([FromBody] EmployeeTeamDTO e)
        {
            var a = db.EmployeeTeam.Add(converter.DTOToEmployeeTeam(e));

            db.SaveChanges();

            return converter.EmployeeTeamToDTO(a.Entity);
        }

        [HttpPut("{id}")]
        public EmployeeTeamDTO EditEmployeeTeam(int id, [FromBody] EmployeeTeam e)
        {
            var empTeam = db.EmployeeTeam.FirstOrDefault(x => x.EmployeeTeamID == e.EmployeeTeamID);

            if (empTeam.Employee != e.Employee)
                empTeam.Employee = e.Employee;
            if (empTeam.Team != e.Team)
                empTeam.Team = e.Team;
            if (empTeam.Role != e.Role)
                empTeam.Role = e.Role;

            db.SaveChanges();
            return converter.EmployeeTeamToDTO(empTeam);
        }

        [HttpDelete("{id}")]
        public void DeleteEmployeeTeam(int id)
        {
            db.EmployeeTeam.Remove(db.EmployeeTeam.First(x => x.EmployeeTeamID == id));
            db.SaveChanges();
        }
    }
}