using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace AnyoneForTennis.Models
{
    public class CoachIndexViewModel
    {
        [DisplayName("Coach Id")]
        public int CoachId { get; set; }

        public string Name { get; set; }
    }
}
