using System;
using System.Collections.Generic;
using System.Text;

namespace ProTick.ResourceDTOs
{
    public class SubprocessDTO
    {
        public int SubprocessID { get; set; }
        public string Description { get; set; }

        public int TeamID { get; set; }
        public int ProcessID { get; set; }
    }
}
