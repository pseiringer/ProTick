using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProTick.ResourceDTOs;
using ProTick.Singletons;
using ProTickDatabase;
using ProTickDatabase.DatabasePOCOs;

namespace ProTick.Controllers
{
    [Route("ProTick/[controller]"), Authorize]

    public class AddressController : Controller
    {
        private ProTickDatabaseContext db;
        private IResourceDTOConverter converter;
        private IDatabaseQueryManager dbm;

        public AddressController([FromServices] ProTickDatabaseContext db, [FromServices] IResourceDTOConverter converter, [FromServices] IDatabaseQueryManager dbm)
        {
            this.db = db;
            this.converter = converter;
            this.dbm = dbm;
        }

        [HttpGet]
        public IEnumerable<AddressDTO> GetAddresses()
        {
            return dbm.FindAllAddresses(true)
                .Select(x => converter.AddressToDTO(x))
                .ToList();
        }
        

        [HttpGet("{id}")]
        public AddressDTO GetAddress(int id)
        {
            return converter.AddressToDTO(dbm.FindAddressByID(id));
        }

        [HttpPost, Authorize(Roles = StaticRoles.Admin)]
        public AddressDTO PostAddress([FromBody] AddressDTO a)
        {
            var add = db.Address.Add(converter.DTOToAddress(a));

            db.SaveChanges();

            return converter.AddressToDTO(add.Entity);
        }

        [HttpPut("{id}"), Authorize(Roles = StaticRoles.Admin)]
        public AddressDTO PutAddress(int id, [FromBody] AddressDTO a)
        {
            var add = db.Address.FirstOrDefault(x => x.AddressID == a.AddressID);

            if (add.Street != a.Street)
                add.Street = a.Street;
            if (add.City != a.City)
                add.City = a.City;
            if (add.Country != a.Country)
                add.Country = a.Country;
            if (add.PostalCode != a.PostalCode)
                add.PostalCode = a.PostalCode;
            if (add.StreetNumber != a.StreetNumber)
                add.StreetNumber = a.StreetNumber;

            db.SaveChanges();
            return converter.AddressToDTO(add);
        }

        [HttpDelete("{id}"), Authorize(Roles = StaticRoles.Admin)]
        public void DeleteAddress(int id)
        {
            db.Address.Remove(db.Address.First(x => x.AddressID == id));
            db.SaveChanges();
        }

    }
}