using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProTick.ResourceDTOs;
using ProTick.Singletons;
using ProTickDatabase;

namespace ProTick.Controllers
{
    [Route("ProTick/[controller]")]

    public class RoleController : Controller
    {
        private ProTickDatabaseContext db;
        private IResourceDTOConverter converter;
        private IDatabaseQueryManager dbm;

        public RoleController([FromServices] ProTickDatabaseContext db, [FromServices] IResourceDTOConverter converter, [FromServices] IDatabaseQueryManager dbm)
        {
            this.db = db;
            this.converter = converter;
            this.dbm = dbm;
        }

        [HttpGet]
        public IEnumerable<RoleDTO> GetRoles()
        {
            return dbm.FindAllRoles(true).Select(x => converter.RoleToDTO(x)).ToList();
        }
    }
}