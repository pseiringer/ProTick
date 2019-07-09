using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProTick.ResourceDTOs;
using ProTickDatabase;
using ProTickDatabase.DatabasePOCOs;

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
            return db.Ticket.Include(x => x.Subprocess).Include(x => x.State).ToList().Select(x => converter.TicketToDTO(x)).ToList();
        }

        [HttpGet("{id}")]
        public TicketDTO GetTicketByID(int id)
        {
            return converter.TicketToDTO(FindTicketByID(id));
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
            var editTicket = FindTicketByID(id);

            bool changesDone = false;
            if (editTicket.Description != ticket.Description)
            {
                editTicket.Description = ticket.Description;
                changesDone = true;
            }
            if (editTicket.State.StateID != ticket.StateID)
            {
                editTicket.State = FindStateByID(ticket.StateID);
                changesDone = true;
            }
            if (editTicket.Subprocess.SubprocessID != ticket.SubprocessID)
            {
                editTicket.Subprocess = FindSubprocessByID(ticket.SubprocessID);
                changesDone = true;
            }

            if (changesDone) db.SaveChanges();
            return converter.TicketToDTO(editTicket);
        }

        [HttpDelete("{id}")]
        public void DeleteTicket(int id)
        {
            var removeTicket = FindTicketByID(id);
            if (removeTicket != null)
            {
                db.Ticket.Remove(removeTicket);
                db.SaveChanges();
            }
        }


        
        private Ticket FindTicketByID(int id)
        {
            var ticket = db.Ticket.Include(x => x.Subprocess).Include(x => x.State).FirstOrDefault(x => x.TicketID == id);
            if (ticket == null) throw new Exception($"Ticket with ID ({id}) was not found");
            return ticket;
        }
        private Subprocess FindSubprocessByID(int id)
        {
            var subprocess = db.Subprocess.FirstOrDefault(x => x.SubprocessID == id);
            if (subprocess == null) throw new Exception($"Subprocess with ID ({id}) was not found");
            return subprocess;
        }
        private State FindStateByID(int id)
        {
            var state = db.State.FirstOrDefault(x => x.StateID == id);
            if (state == null) throw new Exception($"State with ID ({id}) was not found");
            return state;
        }
    }
}
