using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProTick.ResourceDTOs;
using ProTickDatabase;
using ProTickDatabase.DatabasePOCOs;
using ProTick.Singletons;

namespace ProTick.Controllers
{
    [Route("ProTick/[controller]")]
    public class TicketController : Controller
    {
        private ProTickDatabaseContext db;
        private IResourceDTOConverter converter;
        private IDatabaseQueryManager dbm;

        public TicketController([FromServices] ProTickDatabaseContext db, [FromServices] IResourceDTOConverter converter, [FromServices] IDatabaseQueryManager dbm)
        {
            this.db = db;
            this.converter = converter;
            this.dbm = dbm;
        }

        [HttpGet]
        public IEnumerable<TicketDTO> GetAllTickets()
        {
            return dbm.FindAllTickets(true).Select(x => converter.TicketToDTO(x)).ToList();
        }

        [HttpGet("{id}")]
        public TicketDTO GetTicketByID(int id)
        {
            return converter.TicketToDTO(dbm.FindTicketByID(id));
        }

        [HttpPost]
        public TicketDTO PostTicket([FromBody] TicketDTO ticket)
        {
            var newTicket = db.Ticket.Add(converter.DTOToTicket(ticket));
            db.SaveChanges();
            return converter.TicketToDTO(newTicket.Entity);
        }
        
        [HttpPut("{id}")]
        public TicketDTO PutTicket(int id, [FromBody] TicketDTO ticket)
        {
            var editTicket = dbm.FindTicketByID(id);

            bool changesDone = false;
            if (editTicket.Description != ticket.Description)
            {
                editTicket.Description = ticket.Description;
                changesDone = true;
            }
            if (editTicket.State.StateID != ticket.StateID)
            {
                editTicket.State = dbm.FindStateByID(ticket.StateID);
                changesDone = true;
            }
            if (editTicket.Subprocess.SubprocessID != ticket.SubprocessID)
            {
                editTicket.Subprocess = dbm.FindSubprocessByID(ticket.SubprocessID);
                changesDone = true;
            }

            if (changesDone) db.SaveChanges();
            return converter.TicketToDTO(editTicket);
        }

        [HttpDelete("{id}")]
        public void DeleteTicket(int id)
        {
            var removeTicket = dbm.FindTicketByID(id);
            if (removeTicket != null)
            {
                db.Ticket.Remove(removeTicket);
                db.SaveChanges();
            }
        }
    }
}
