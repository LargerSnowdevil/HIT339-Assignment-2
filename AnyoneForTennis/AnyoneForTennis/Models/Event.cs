using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AnyoneForTennis.Models
{
    public class Event
    {
        [Key]
        public int EventId { get; set; }

        public string Name { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public string Location { get; set; }

        public Coach RunningCoach { get; set; }

        public List<EventMember> EventMembers { get; set; }
    }
}
