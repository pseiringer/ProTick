using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProTick.Singletons
{
    public interface IHasher
    {
        string HashPassword (string password);
    }
}
