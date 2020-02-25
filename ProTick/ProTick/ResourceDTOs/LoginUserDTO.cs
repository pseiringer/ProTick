using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProTick.ResourceDTOs
{
    public class LoginUserDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string OldPassword { get; set; }
    }
}
