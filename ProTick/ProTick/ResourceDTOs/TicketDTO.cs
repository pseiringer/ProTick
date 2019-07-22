using System;
using System.Collections.Generic;
using System.Text;

namespace ProTick.ResourceDTOs
{
    public class TicketDTO
    {
        public int TicketID { get; set; }
        public string Description { get; set; }
        public string Note { get; set; }

        public int SubprocessID { get; set; }
        public int StateID { get; set; }
    }
}
