﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProTick.ResourceDTOs;
using ProTickDatabase;
using ProTickDatabase.DatabasePOCOs;
using ProTick.Singletons;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ProTick.Controllers
{
    [Route("ProTick/[controller]"), Authorize]
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

        [HttpGet, Authorize(Roles = StaticRoles.Admin)]
        public IEnumerable<TicketDTO> GetTickets()
        {
            return dbm.FindAllTickets(true).Select(x => converter.TicketToDTO(x)).ToList();
        }
        
        [HttpGet("Username/{user}")]
        public IEnumerable<TicketDTO> GetTicketsByUsername(string user)
        {
            string loggedInUser = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            string loggedInRole = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value;
            if (loggedInUser == user || loggedInRole == StaticRoles.Admin)
                return dbm.FindAllTicketsByUsername(user).Select(x => converter.TicketToDTO(x)).ToList();

            return new List<TicketDTO>();
        }

        [HttpGet("{id}")]
        public TicketDTO GetTicket(int id)
        {
            return converter.TicketToDTO(dbm.FindTicketByID(id));
        }

        [HttpPost, Authorize(Roles = StaticRoles.Admin)]
        public TicketDTO PostTicket([FromBody] TicketDTO ticket)
        {
            var newTicket = db.Ticket.Add(converter.DTOToTicket(ticket));
            db.SaveChanges();
            return converter.TicketToDTO(newTicket.Entity);
        }
        
        [HttpPut("{id}"), Authorize(Roles = StaticRoles.Admin)]
        public TicketDTO PutTicket(int id, [FromBody] TicketDTO ticket)
        {
            var editTicket = dbm.FindTicketByID(id);

            bool changesMade = false;
            if (ticket.Description != null && ticket.Description != "" && editTicket.Description != ticket.Description)
            {
                editTicket.Description = ticket.Description;
                changesMade = true;
            }
            if (ticket.Note != null && ticket.Note != "" && editTicket.Note != ticket.Note)
            {
                editTicket.Note = ticket.Note;
                changesMade = true;
            }
            if (ticket.StateID > 0 && editTicket.State.StateID != ticket.StateID)
            {
                editTicket.State = dbm.FindStateByID(ticket.StateID);
                changesMade = true;
            }
            if (ticket.SubprocessID > 0 && 
                ((editTicket.Subprocess != null 
                    && editTicket.Subprocess.SubprocessID != ticket.SubprocessID) 
                    || editTicket.Subprocess == null))
            {
                editTicket.Subprocess = dbm.FindSubprocessByID(ticket.SubprocessID);
                changesMade = true;
            }
            else if (ticket.SubprocessID == -1 && editTicket.Subprocess != null)
            {
                editTicket.Subprocess = null;
                changesMade = true;
            }

            if (changesMade) db.SaveChanges();
            return converter.TicketToDTO(editTicket);
        }

        [HttpDelete("{id}"), Authorize(Roles = StaticRoles.Admin)]
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
