using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProTick.ResourceDTOs;
using ProTick.Singletons;
using ProTickDatabase;

namespace ProTick.Controllers
{

    [Route("ProTick/[controller]"), Authorize(Roles = StaticRoles.Admin)]
    public class ParentChildRelationController : Controller
    {
        private ProTickDatabaseContext db;
        private IResourceDTOConverter converter;
        private IDatabaseQueryManager dbm;

        public ParentChildRelationController([FromServices] ProTickDatabaseContext db, [FromServices] IResourceDTOConverter converter, [FromServices] IDatabaseQueryManager dbm)
        {
            this.db = db;
            this.converter = converter;
            this.dbm = dbm;
        }

        [HttpGet]
        public IEnumerable<ParentChildRelationDTO> GetAllParentChildRelations()
        {
            return dbm.FindAllParentChildRelations(true).Select(x => converter.ParentChildRelationToDTO(x)).ToList();
        }

        [HttpGet("{id}")]
        public ParentChildRelationDTO GetParentChildRelationByID(int id)
        {
            return converter.ParentChildRelationToDTO(dbm.FindParentChildRelationByID(id));
        }

        [HttpPost]
        public ParentChildRelationDTO PostParentChildRelation([FromBody] ParentChildRelationDTO parentChildRelation)
        {
            var newParentChildRelation = db.ParentChildRelation.Add(converter.DTOToParentChildRelation(parentChildRelation));
            db.SaveChanges();
            return converter.ParentChildRelationToDTO(newParentChildRelation.Entity);
        }

        [HttpPut("{id}")]
        public ParentChildRelationDTO PutParentChildRelation(int id, [FromBody] ParentChildRelationDTO ticket)
        {
            var editParentChildRelation = dbm.FindParentChildRelationByID(id);

            bool changesMade = false;

            if ((editParentChildRelation.Parent == null && ticket.ParentID != -1) || (editParentChildRelation.Parent != null && (editParentChildRelation.Parent.SubprocessID != ticket.ParentID)))
            {
                editParentChildRelation.Parent = dbm.FindSubprocessByID(ticket.ParentID);
                changesMade = true;
            }
            if ((editParentChildRelation.Child == null && ticket.ChildID != -1) || (editParentChildRelation.Child != null && (editParentChildRelation.Child.SubprocessID != ticket.ChildID)))
            {
                editParentChildRelation.Child = dbm.FindSubprocessByID(ticket.ChildID);
                changesMade = true;
            }

            if (changesMade) db.SaveChanges();
            return converter.ParentChildRelationToDTO(editParentChildRelation);
        }

        [HttpDelete("{id}")]
        public void DeleteTicket(int id)
        {
            var removeParentChildRelation = dbm.FindParentChildRelationByID(id);
            if (removeParentChildRelation != null)
            {
                db.ParentChildRelation.Remove(removeParentChildRelation);
                db.SaveChanges();
            }
        }
    }
}