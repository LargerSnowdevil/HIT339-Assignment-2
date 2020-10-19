using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AnyoneForTennis.Models
{
    public class EventDetailsViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public string Location { get; set; }

        [DisplayName("Coach")]
        public string RunningCoach { get; set; }

        public int CoachId { get; set; }

        [DisplayName("Enrolled Members")]
        public List<string> Members { get; set; }
    }
}
