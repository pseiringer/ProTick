using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProTick
{
    public static class StaticRoles
    {
        public const string Admin = "Admin";
        public const string Employee = "Employee";

        public static string Combine(params string[] roles)
        {
            return string.Join(", ", roles);
        }
    }
}
