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
    public class ProcessController : Controller
    {
        private ProTickDatabaseContext db;
        private IResourceDTOConverter converter;
        private IDatabaseQueryManager dbm;

        public ProcessController([FromServices] ProTickDatabaseContext db, [FromServices] IResourceDTOConverter converter, [FromServices] IDatabaseQueryManager dbm)
        {
            this.db = db;
            this.converter = converter;
            this.dbm = dbm;
        }


        [HttpGet("{id}")]
        public ProcessDTO GetProcess([FromServices] ProTickDatabaseContext db, int id)
        {
            return converter.ProcessToDTO(dbm.FindProcessByID(id));
        }

        [HttpGet]
        public IEnumerable<ProcessDTO> GetProcesses([FromServices] ProTickDatabaseContext db)
        {
            return dbm.FindAllProcesses(true).Select(x => converter.ProcessToDTO(x)).ToList();
        }

        [HttpGet("hasSubprocess={hasSub}")]
        public IEnumerable<ProcessDTO> GetProcessesWithSubprocess([FromServices] ProTickDatabaseContext db, bool hasSub)
        {
            return dbm.FindAllProcessesWithSubprocess(hasSub).Select(x => converter.ProcessToDTO(x)).ToList();
        }

        [HttpGet("{id}/Subprocesses")]
        public IEnumerable<SubprocessDTO> GetSubprocessesByProcessID([FromServices] ProTickDatabaseContext db, int id)
        {
            return dbm.FindAllSubprocesses(true).Where(x => x.Process.ProcessID == id).Select(x => converter.SubprocessToDTO(x)).ToList();
        }

        [HttpGet("{id}/ParentChildRelations")]
        public IEnumerable<ParentChildRelationDTO> GetParentChildRelationsByProcessID([FromServices] ProTickDatabaseContext db, int id)
        {
            return dbm.FindAllParentChildRelationsOfProcess(id).Select(x => converter.ParentChildRelationToDTO(x)).ToList();
        }

        [HttpPost]
        public ProcessDTO NewProcess([FromServices] ProTickDatabaseContext db, [FromBody] ProcessDTO p)
        {
            Console.WriteLine(p);

            var a = db.Process.Add(converter.DTOToProcess(p));

            Console.WriteLine(p.Description);

            db.SaveChanges();

            return converter.ProcessToDTO(a.Entity);
        }

        [HttpPut("{id}")]
        public ProcessDTO EditProcess([FromServices] ProTickDatabaseContext db, int id, [FromBody] Process p)
        {
            var pr = db.Process.FirstOrDefault(x => x.ProcessID == p.ProcessID);

            if(pr.Description != p.Description)
            pr.Description = p.Description;

            db.SaveChanges();
            return converter.ProcessToDTO(pr);
        }

        [HttpDelete("{id}")]
        public void DeleteProcess([FromServices] ProTickDatabaseContext db, int id)
        {
            db.Process.Remove(db.Process.First(x => x.ProcessID == id));
            db.SaveChanges();
        }
    }
}