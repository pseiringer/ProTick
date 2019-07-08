using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProTickDatabase;
using ProTickDatabase.DatabasePOCOs;

namespace ProTick.Controllers
{
    [Route("ProTick/[controller]")]
    public class ProcessController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public List<Process> Test([FromServices] ProTickDatabaseContext db)
        {
            //db.Process.Add(new Process { Description = "test_process_1" });
            //db.Process.Add(new Process { Description = "test_process_2" });
            //db.SaveChanges();

            return db.Process.ToList();
        }

        [HttpGet("{id}")]
        public Process GetProcess([FromServices] ProTickDatabaseContext db, int id)
        {
            return db.Process.First(x => x.ProcessID == id);
        }

        [HttpGet]
        public List<Process> GetProcesses([FromServices] ProTickDatabaseContext db)
        {
            return db.Process.ToList();
        }

        [HttpPost("{p}")]
        public List<Process> NewProcess([FromServices] ProTickDatabaseContext db, Process p)
        {
            return db.Process.ToList();
        }
    }
}