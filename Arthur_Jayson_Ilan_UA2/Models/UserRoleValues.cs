using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arthur_Jayson_Ilan_UA2.Services;

namespace Arthur_Jayson_Ilan_UA2.Models
{
    public static class UserRoleValues
    {
        public static List<UserRole> Roles { get; } = new List<UserRole>()
        {
            UserRole.SuperAdmin,
            UserRole.Administrator,
            UserRole.Librarian,
            UserRole.Client
        };
    }
}
