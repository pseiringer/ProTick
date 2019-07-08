using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProTick.ResourceDTOs;
using ProTickDatabase;

namespace ProTick.Controllers
{
    [Route("ProTick/[controller]")]
    public class TicketController : Controller
    {
        private ResourceDTOConverter converter = new ResourceDTOConverter();
        private ProTickDatabaseContext db = null;

        public TicketController([FromServices] ProTickDatabaseContext db)
        {
            this.db = db;
            this.converter = new ResourceDTOConverter(db);
        }

        [HttpGet]
        public IEnumerable<TicketDTO> GetAllTickets()
        {
            return db.Ticket.ToList().Select(x => converter.TicketToDTO(x)).ToList();
        }

        [HttpGet("{id}")]
        public TicketDTO GetTicketByID(int id)
        {
            return converter.TicketToDTO(db.Ticket.FirstOrDefault(x => x.TicketID == id));
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
            var editTicket = db.Ticket.Include(x => x.State).FirstOrDefault(x => x.TicketID == id);
            if (editTicket == null) throw new Exception("Ticket was not found");
            if (editTicket.State.StateID != ticket.StateID) editTicket.State.StateID = ticket.StateID;
            //TODO add other changes
            db.SaveChanges();
            return converter.TicketToDTO(editTicket);
        }

        [HttpDelete("{id}")]
        public void DeleteTicket(int id)
        {
            var removeTicket = db.Ticket.FirstOrDefault(x => x.TicketID == id);
            if (removeTicket != null)
            {
                db.Ticket.Remove(removeTicket);
                db.SaveChanges();
            }
        }

    }
}