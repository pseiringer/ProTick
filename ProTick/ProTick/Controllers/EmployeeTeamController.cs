using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProTick.ResourceDTOs;
using ProTick.Singletons;
using ProTickDatabase;
using ProTickDatabase.DatabasePOCOs;

namespace ProTick.Controllers
{
    [Route("ProTick/[controller]"), Authorize]
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


        [HttpPost, Authorize(Roles = StaticRoles.Admin)]
        public EmployeeTeamDTO NewEmployeeTeam([FromBody] EmployeeTeamDTO e)
        {
            var a = db.EmployeeTeam.Add(converter.DTOToEmployeeTeam(e));

            db.SaveChanges();

            return converter.EmployeeTeamToDTO(a.Entity);
        }

        [HttpPut("{id}"), Authorize(Roles = StaticRoles.Admin)]
        public EmployeeTeamDTO EditEmployeeTeam(int id, [FromBody] EmployeeTeamDTO e)
        {
            var empTeam = dbm.FindEmployeeTeamByID(id);

            if (empTeam.Employee.EmployeeID != e.EmployeeID)
                empTeam.Employee = dbm.FindEmployeeByID(e.EmployeeID);
            if (empTeam.Team.TeamID != e.TeamID)
                empTeam.Team = dbm.FindTeamByID(e.TeamID);

            db.SaveChanges();
            return converter.EmployeeTeamToDTO(empTeam);
        }

        [HttpDelete("{id}"), Authorize(Roles = StaticRoles.Admin)]
        public void DeleteEmployeeTeam(int id)
        {
            db.EmployeeTeam.Remove(db.EmployeeTeam.First(x => x.EmployeeTeamID == id));
            db.SaveChanges();
        }

        [HttpDelete("{tId}/{eId}"), Authorize(Roles = StaticRoles.Admin)]
        public void DeleteEmployeeTeamByTeamAndEmpId(int tId, int eId)
        {
            var empTeam = db.EmployeeTeam.FirstOrDefault(x => x.Team.TeamID == tId && x.Employee.EmployeeID == eId);
            if (empTeam == null) return;
            db.EmployeeTeam.Remove(empTeam);
            db.SaveChanges();
        }
    }
}