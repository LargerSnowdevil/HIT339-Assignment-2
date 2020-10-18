using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AnyoneForTennis.Models
{
    public class Member
    {
        [Key]
        public int MemeberId { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public bool IsAdmin { get; set; }

        public List<EventMember> EventMembers { get; set; }
    }
}
