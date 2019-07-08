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
    public class ProcessController : Controller
    {
        private ResourceDTOConverter converter = new ResourceDTOConverter(null);

        public IActionResult Index()
        {
            return View();
        }

        public IEnumerable<Process> Test([FromServices] ProTickDatabaseContext db)
        {
            db.Process.Add(new Process { Description = "test_process_1" });
            db.Process.Add(new Process { Description = "test_process_2" });
            db.SaveChanges();

            return db.Process.ToList();
        }

        [HttpGet("{id}")]
        public ProcessDTO GetProcess([FromServices] ProTickDatabaseContext db, int id)
        {
            return converter.ProcessToDTO(db.Process.First(x => x.ProcessID == id));
        }

        [HttpGet]
        public IEnumerable<ProcessDTO> GetProcesses([FromServices] ProTickDatabaseContext db)
        {
            return db.Process.Select(x => converter.ProcessToDTO(x)).ToList();
        }

        [HttpPost("{p}")]
        public ProcessDTO NewProcess([FromServices] ProTickDatabaseContext db, Process p)
        {
            var a = db.Process.Add(p);
            
            db.SaveChanges();

            return converter.ProcessToDTO(a.Entity);
        }

        [HttpPut("{id}")]
        public ProcessDTO Edit([FromServices] ProTickDatabaseContext db, int id, [FromBody] Process p)
        {
            var pr = db.Process.FirstOrDefault(x => x.ProcessID == p.ProcessID);

            if(pr.Description != p.Description)
            pr.Description = p.Description;

            db.SaveChanges();
            return converter.ProcessToDTO(pr);
        }

        [HttpDelete("{id}")]
        public void Delete([FromServices] ProTickDatabaseContext db, int id)
        {
            db.Process.Remove(db.Process.First(x => x.ProcessID == id));
            db.SaveChanges();
        }
    }
}