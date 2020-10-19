using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace AnyoneForTennis.Models
{
    public class RegisterViewModel
    {
        [NotNull]
        public string Username { get; set; }

        [NotNull]
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
