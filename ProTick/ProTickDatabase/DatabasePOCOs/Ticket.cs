using System;
using System.Collections.Generic;
using System.Text;

namespace ProTickDatabase.DatabasePOCOs
{
    public class Ticket
    {
        public int TicketID { get; set; }
        public string Description { get; set; }
        public string Note { get; set; }

        public virtual Subprocess Subprocess { get; set; }
        public virtual State State { get; set; }
    }
}
