using ProTick.ResourceDTOs;
using ProTickDatabase.DatabasePOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProTick
{
    public class ResourceDTOConverter
    {
        public AddressResourceDTO AddressToDTO(Address address)
        {
            return new AddressResourceDTO() { AddressID = address.AddressID, Country = address.Country, PostalCode = address.PostalCode, Street = address.Street, StreetNumber = address.StreetNumber };
        }
        public Address ResourceToAddress(AddressResourceDTO address)
        {
            return new Address() { AddressID = address.AddressID, Country = address.Country, PostalCode = address.PostalCode, Street = address.Street, StreetNumber = address.StreetNumber };
        }
    }
}
