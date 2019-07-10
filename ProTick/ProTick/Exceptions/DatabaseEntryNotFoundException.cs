using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProTick.Exceptions
{
    class DatabaseEntryNotFoundException : Exception
    {
        public DatabaseEntryNotFoundException(string message) : base(message)
        {
        }
    }
}
