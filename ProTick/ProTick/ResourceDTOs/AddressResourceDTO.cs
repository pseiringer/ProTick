using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProTick.ResourceDTOs
{
    public class AddressResourceDTO
    {
        public int AddressID { get; set; }
        public string Street { get; set; }
        public string StreetNumber { get; set; }
        public int PostalCode { get; set; }
        public string Country { get; set; }

    }
}
