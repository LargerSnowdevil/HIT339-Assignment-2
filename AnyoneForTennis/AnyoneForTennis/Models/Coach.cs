using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AnyoneForTennis.Models
{
    public class Coach
    {
        [Key]
        public int CoachId { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public string Biography { get; set; }

        public string Username { get; set; }

        public List<Event> RunningEvents { get; set; }
    }
}
