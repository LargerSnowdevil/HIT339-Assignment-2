using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnyoneForTennis.Models
{
    public class EventMember
    {
        public int EventId { get; set; }

        public Event Event { get; set; }

        public int MemberId { get; set; }

        public Member Member { get; set; }
    }
}
