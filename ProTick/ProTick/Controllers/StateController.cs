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
    [Route("ProTick/[controller]"), Authorize]
    public class StateController : Controller
    {
        private ProTickDatabaseContext db;
        private IResourceDTOConverter converter;
        private IDatabaseQueryManager dbm;

        public StateController([FromServices] ProTickDatabaseContext db, [FromServices] IResourceDTOConverter converter, [FromServices] IDatabaseQueryManager dbm)
        {
            this.db = db;
            this.converter = converter;
            this.dbm = dbm;
        }

        [HttpGet]
        public IEnumerable<StateDTO> GetAllState()
        {
            return dbm.FindAllStates(true).Select(x => converter.StateToDTO(x)).ToList();
        }

        [HttpGet("{id}")]
        public StateDTO GetStateByID(int id)
        {
            return converter.StateToDTO(dbm.FindStateByID(id));
        }

        [HttpGet("{id}/Tickets")]
        public IEnumerable<TicketDTO> GetTicketsByStateID(int id)
        {
            return dbm.FindAllTicketsByStateID(id).Select(x => converter.TicketToDTO(x)).ToList();
        }

        [HttpPost]
        public StateDTO PostState([FromBody] StateDTO state)
        {
            var newState = db.State.Add(converter.DTOToState(state));
            db.SaveChanges();
            return converter.StateToDTO(newState.Entity);
        }

        [HttpPut("{id}")]
        public StateDTO PutState(int id, [FromBody] StateDTO state)
        {
            var editState = dbm.FindStateByID(id);

            bool changesMade = false;
            if (editState.Description != state.Description)
            {
                editState.Description = state.Description;
                changesMade = true;
            }

            if (changesMade) db.SaveChanges();
            return converter.StateToDTO(editState);
        }

        [HttpDelete("{id}")]
        public void DeleteState(int id)
        {
            var removeState = dbm.FindStateByID(id);
            if (removeState != null)
            {
                db.State.Remove(removeState);
                db.SaveChanges();
            }
        }
    }
}