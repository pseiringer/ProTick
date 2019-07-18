using System;
using System.Collections.Generic;
using System.Text;

namespace ProTickDatabase.DatabasePOCOs
{
    public class Address
    {
        public int AddressID { get; set; }
        public string Street { get; set; } 
        public string StreetNumber { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }      
        public string City { get; set; }
        

    }
}
