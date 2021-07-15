using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace meetingsAPI.Models
{
    public class ConferenceRoom
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public ICollection<Meeting> Meetings { get; set; } = new List<Meeting>();
    }
}
