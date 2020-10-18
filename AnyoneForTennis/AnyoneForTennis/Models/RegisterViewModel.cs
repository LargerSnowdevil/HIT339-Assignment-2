using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnyoneForTennis.Models
{
    public class RegisterViewModel
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public bool IsCoach { get; set; }

        //^^Both--vvMember-----------------------------------------

        public string Email { get; set; }

        //^^Memeber--vvCoach-------------------------------

        public string Name { get; set; }

        public int Age { get; set; }

        public string Biography { get; set; }
    }
}
