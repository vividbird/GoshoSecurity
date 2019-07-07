using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoshoSecurityAPI.Models
{
    public class LoggedUser
    {
        public string Name { get; set; }

        public string Username { get; set; }
       
        public string Email { get; set; }

        public string Role { get; set; }

        public string Token { get; set; }
    }
}
