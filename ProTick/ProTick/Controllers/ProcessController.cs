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
        public ProcessDTO GetProcess(int id)
        {
            return converter.ProcessToDTO(dbm.FindProcessByID(id));
        }

        [HttpGet]
        public IEnumerable<ProcessDTO> GetProcesses()
        {
            return dbm.FindAllProcesses(true).Select(x => converter.ProcessToDTO(x)).ToList();
        }

        [HttpGet("hasSubprocess={hasSub}")]
        public IEnumerable<ProcessDTO> GetProcessesWithSubprocess(bool hasSub)
        {
            return dbm.FindAllProcessesWithSubprocess(hasSub)
                .Select(x => converter.ProcessToDTO(x))
                .ToList();
        }

        [HttpGet("{id}/Subprocesses")]
        public IEnumerable<SubprocessDTO> GetSubprocessesByProcessID(int id)
        {
            return dbm.FindAllSubprocesses(true)
                .Where(x => x.Process.ProcessID == id)
                .Select(x => converter.SubprocessToDTO(x))
                .ToList();
        }

        [HttpGet("{id}/ParentChildRelations")]
        public IEnumerable<ParentChildRelationDTO> GetParentChildRelationsByProcessID(int id)
        {
            return dbm.FindAllParentChildRelationsOfProcess(id)
                .Select(x => converter.ParentChildRelationToDTO(x))
                .ToList();
        }

        [HttpPost, Authorize(Roles = StaticRoles.Admin)]
        public ProcessDTO PostProcess([FromBody] ProcessDTO p)
        {
            Console.WriteLine(p);

            var a = db.Process.Add(converter.DTOToProcess(p));

            Console.WriteLine(p.Description);

            db.SaveChanges();

            return converter.ProcessToDTO(a.Entity);
        }

        [HttpPut("{id}"), Authorize(Roles = StaticRoles.Admin)]
        public ProcessDTO PutProcess(int id, [FromBody] ProcessDTO p)
        {
            var pr = db.Process.FirstOrDefault(x => x.ProcessID == p.ProcessID);

            if (pr.Description != p.Description)
                pr.Description = p.Description;

            db.SaveChanges();
            return converter.ProcessToDTO(pr);
        }

        [HttpDelete("{id}"), Authorize(Roles = StaticRoles.Admin)]
        public void DeleteProcess(int id)
        {
            db.Subprocess.Where(x => x.Process.ProcessID == id)
                    .ToList()
                    .ForEach(subprocess =>
                    {
                        db.Ticket.Where(y => y.Subprocess.SubprocessID == subprocess.SubprocessID)
                            .ToList()
                            .ForEach(ticket => db.Ticket.Remove(ticket));

                        db.ParentChildRelation.Where(pcl => pcl.Child.SubprocessID == subprocess.SubprocessID || pcl.Parent.SubprocessID == subprocess.SubprocessID)
                            .ToList()
                            .ForEach(pcl => db.ParentChildRelation.Remove(pcl));

                        db.Subprocess.Remove(subprocess);
                    });

            db.Process.Remove(db.Process.First(x => x.ProcessID == id));
            db.SaveChanges();
        }
    }
}