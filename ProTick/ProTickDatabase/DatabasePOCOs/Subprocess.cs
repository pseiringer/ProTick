using System;
using System.Collections.Generic;
using System.Text;

namespace ProTickDatabase.DatabasePOCOs
{
    public class Subprocess
    {
        public int SubprocessID { get; set; }
        public string Description { get; set; }

        public virtual Team Team { get; set; }
        public virtual Process Process { get; set; }
    }
}
