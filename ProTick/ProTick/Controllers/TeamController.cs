using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
    public class TeamController : Controller
    {
        private ProTickDatabaseContext db;
        private IResourceDTOConverter converter;
        private IDatabaseQueryManager dbm;

        public TeamController([FromServices] ProTickDatabaseContext db, [FromServices] IResourceDTOConverter converter, [FromServices] IDatabaseQueryManager dbm)
        {
            this.db = db;
            this.converter = converter;
            this.dbm = dbm;
        }


        [HttpGet("{id}")]
        public TeamDTO GetTeam(int id)
        {
            return converter.TeamToDTO(dbm.FindTeamByID(id));
        }

        [HttpGet]
        public IEnumerable<TeamDTO> GetTeams()
        {
            return dbm.FindAllTeams(true).Select(x => converter.TeamToDTO(x)).ToList();
        }

        [HttpGet("Username/{user}")]
        public IEnumerable<TeamDTO> GetTeamsByUsername(string user)
        {
            string loggedInUser = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            string loggedInRole = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value;
            if (loggedInUser == user || loggedInRole == StaticRoles.Admin)
                return dbm.FindAllTeamsByUsername(user).Select(x => converter.TeamToDTO(x)).ToList();

            return new List<TeamDTO>();
        }

        [HttpGet("{id}/Tickets")]
        public IEnumerable<TicketDTO> GetTicketsByTeamID(int id)
        {
            return dbm.FindAllTicketsByTeamID(id)
                .Select(x => converter.TicketToDTO(x))
                .ToList();
        }

        [HttpGet("{id}/Employees")]
        public IEnumerable<EmployeeDTO> GetEmployeesByTeamID(int id)
        {
            return dbm.FindAllEmployeeTeams(true)
                .Where(x => x.Team.TeamID == id)
                .SelectMany(x => dbm.FindAllEmployees(true)
                .Where(y => x.Employee.EmployeeID == y.EmployeeID))
                .Distinct()
                .Select(x => converter.EmployeeToDTO(x))
                .ToList();
        }

        [HttpGet("{id}/EmployeeTeams")]
        public IEnumerable<EmployeeTeamDTO> GetEmployeeTeamsByTeamID(int id)
        {
            return dbm.FindEmployeeTeamsByTeamID(id)
                .Select(x => converter.EmployeeTeamToDTO(x))
                .ToList();

        }

        [HttpPost, Authorize(Roles = StaticRoles.Admin)]
        public TeamDTO PostTeam([FromBody] TeamDTO t)
        {
            var a = db.Team.Add(converter.DTOToTeam(t));

            db.SaveChanges();

            return converter.TeamToDTO(a.Entity);
        }

        [HttpPut("{id}"), Authorize(Roles = StaticRoles.Admin)]
        public TeamDTO PutTeam(int id, [FromBody] TeamDTO t)
        {
            var team = db.Team.FirstOrDefault(x => x.TeamID == t.TeamID);

            if (team.Description != t.Description)
                team.Description = t.Description;
            if (team.Abbreviation != t.Abbreviation)
                team.Abbreviation = t.Abbreviation;

            db.SaveChanges();
            return converter.TeamToDTO(team);
        }

        [HttpDelete("{id}"), Authorize(Roles = StaticRoles.Admin)]
        public void DeleteTeam(int id)
        {
            var et = dbm.FindEmployeeTeamsByTeamID(id);
            for (int i = 0; i < et.Count; i++)
            {
                db.EmployeeTeam.Remove(et[i]);
            }
            db.Team.Remove(db.Team.First(x => x.TeamID == id));
            db.SaveChanges();
        }
    }
}