using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProTick.ResourceDTOs;
using ProTickDatabase;
using ProTickDatabase.DatabasePOCOs;

namespace ProTick.Controllers
{

    [Route("ProTick/[controller]")]
    public class TeamController : Controller
    {
        private ResourceDTOConverter converter = new ResourceDTOConverter(null);

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("{id}")]
        public TeamDTO GetTeam([FromServices] ProTickDatabaseContext db, int id)
        {
            return converter.TeamToDTO(db.Team.First(x => x.TeamID == id));
        }

        [HttpGet]
        public IEnumerable<TeamDTO> GetTeams([FromServices] ProTickDatabaseContext db)
        {
            return db.Team.Select(x => converter.TeamToDTO(x)).ToList();
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