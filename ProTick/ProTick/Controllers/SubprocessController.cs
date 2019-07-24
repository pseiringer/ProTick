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
    public class SubprocessController : Controller
    {
        private ProTickDatabaseContext db;
        private IResourceDTOConverter converter;
        private IDatabaseQueryManager dbm;

        public SubprocessController([FromServices] ProTickDatabaseContext db, [FromServices] IResourceDTOConverter converter, [FromServices] IDatabaseQueryManager dbm)
        {
            this.db = db;
            this.converter = converter;
            this.dbm = dbm;
        }


        [HttpGet("{id}")]
        public SubprocessDTO GetSubprocess([FromServices] ProTickDatabaseContext db, int id)
        {
            return converter.SubprocessToDTO(dbm.FindSubprocessByID(id));
        }

        [HttpGet]
        public IEnumerable<SubprocessDTO> GetSubprocesses([FromServices] ProTickDatabaseContext db)
        {
            return dbm.FindAllSubprocesses(true).Select(x => converter.SubprocessToDTO(x)).ToList();
        }

        [HttpGet("{id}/Children")]
        public IEnumerable<SubprocessDTO> GetChildrenOfSubprocess([FromServices] ProTickDatabaseContext db, int id)
        {
            return dbm.FindAllChildrenBySubprocessID(id)
                .Select(x => 
                {
                    if (x != null) return converter.SubprocessToDTO(x);
                    return null;
                })
                .ToList();
        }

        [HttpPost]
        public SubprocessDTO NewSubprocess([FromServices] ProTickDatabaseContext db, [FromBody] SubprocessDTO s)
        {
            var a = db.Subprocess.Add(converter.DTOToSubprocess(s));

            Console.WriteLine(s.Description);

            db.SaveChanges();

            return converter.SubprocessToDTO(a.Entity);
        }

        [HttpPut("{id}")]
        public SubprocessDTO EditSubprocess([FromServices] ProTickDatabaseContext db, int id, [FromBody] Subprocess s)
        {
            var sp = db.Subprocess.FirstOrDefault(x => x.SubprocessID == s.SubprocessID);

            if (sp.Description != s.Description)
                sp.Description = s.Description;

            db.SaveChanges();
            return converter.SubprocessToDTO(sp);
        }

        [HttpDelete("{id}")]
        public void DeleteSubprocess([FromServices] ProTickDatabaseContext db, int id)
        {
            db.Subprocess.Remove(db.Subprocess.First(x => x.SubprocessID == id));
            db.SaveChanges();
        }
    }
}