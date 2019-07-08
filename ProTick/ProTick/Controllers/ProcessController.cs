using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProTickDatabase;
using ProTickDatabase.DatabasePOCOs;

namespace ProTick.Controllers
{
    [Route("[controller]/[action]")]
    public class ProcessController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public List<Process> Test([FromServices] ProTickDatabaseContext db)
        {
            db.Process.Add(new Process { Description = "test_process_1"});
            db.Process.Add(new Process { Description = "test_process_2" });
            db.SaveChanges();

            return db.Process.ToList();
        }
    }
}