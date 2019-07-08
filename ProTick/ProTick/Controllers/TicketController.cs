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
        private ResourceDTOConverter converter = new ResourceDTOConverter();

        [HttpGet]
        public IEnumerable<AddressDTO> GetAllTickets([FromServices] ProTickDatabaseContext db)
        {
            return db.Address.ToList().Select(x => converter.AddressToDTO(x)).ToList();
        }

        [HttpGet("{id}")]
        public AddressDTO GetTicketByID([FromServices] ProTickDatabaseContext db, int id)
        {
            return converter.AddressToDTO(db.Address.FirstOrDefault(x => x.AddressID == id));
        }

        [HttpPost]
        public AddressDTO PostTicket([FromServices] ProTickDatabaseContext db, [FromBody] AddressDTO address)
        {
            var newAddress = db.Address.Add(converter.ResourceToAddress(address));
            db.SaveChanges();
            return converter.AddressToDTO(newAddress.Entity);
        }

        [HttpPut("{id}")]
        public AddressDTO PostTicket([FromServices] ProTickDatabaseContext db, int id, [FromBody] AddressDTO address)
        {
            var oldAddress = db.Address.FirstOrDefault(x => x.AddressID == id);
            if (oldAddress == null) throw new 
            if (oldAddress.AddressID != address.AddressID)
            {

            }
            db.SaveChanges();
            return converter.AddressToDTO(newAddress.Entity);
        }

    }
}