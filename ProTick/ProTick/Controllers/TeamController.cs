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
        public TeamDTO GetTeam([FromServices] ProTickDatabaseContext db, int id)
        {
            return converter.TeamToDTO(dbm.FindTeamByID(id));
        }

        [HttpGet]
        public IEnumerable<TeamDTO> GetTeams([FromServices] ProTickDatabaseContext db)
        {
            return dbm.FindAllTeams(true).Select(x => converter.TeamToDTO(x)).ToList();
        }

        [HttpPost("{t}")]
        public TeamDTO NewTeam([FromServices] ProTickDatabaseContext db, Team t)
        {
            var a = db.Team.Add(t);

            db.SaveChanges();

            return converter.TeamToDTO(a.Entity);
        }

        [HttpPut("{id}")]
        public TeamDTO EditTeam([FromServices] ProTickDatabaseContext db, int id, [FromBody] Team t)
        {
            var team = db.Team.FirstOrDefault(x => x.TeamID == t.TeamID);

            if (team.Description != team.Description)
                team.Description = team.Description;

            db.SaveChanges();
            return converter.TeamToDTO(team);
        }

        [HttpDelete("{id}")]
        public void DeleteTeam([FromServices] ProTickDatabaseContext db, int id)
        {
            db.Team.Remove(db.Team.First(x => x.TeamID == id));
            db.SaveChanges();
        }
    }
}