using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProTick.ResourceDTOs;
using ProTickDatabase;

namespace ProTick.Controllers
{
    [Route("ProTick/[controller]")]
    public class TicketController : Controller
    {
        private ResourceDTOConverter converter = new ResourceDTOConverter(null);

        [HttpGet]
        public IEnumerable<AddressDTO> GetAllTickets([FromServices] ProTickDatabaseContext db)
        {
            return db.Address.ToList().Select(x => converter.AddressToDTO(x)).ToList();
        }

        [HttpGet("{id}")]
        public IEnumerable<AddressDTO> GetTicketByID([FromServices] ProTickDatabaseContext db, int id)
        {
            return db.Address.Where(x => x.AddressID == id).ToList().Select(x => converter.AddressToDTO(x)).ToList();
        }
        
        //[HttpPost("Ticket")]
        //public void PostTicket([FromServices] ProTickDatabaseContext db, AddressDTO address)
        //{
        //    db.Address.Add(converter.ResourceToAddress(address));
        //    db.SaveChanges();
        //}
        
        [HttpPost("Ticket")]
        public void PostTicket([FromServices] ProTickDatabaseContext db, AddressDTO address)
        {
            db.Address.Add(converter.DTOToAddress(address));
            db.SaveChanges();
        }
    }
}
