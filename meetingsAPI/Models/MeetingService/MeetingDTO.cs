using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace meetingsAPI.Models.MeetingService
{
    public class MeetingDTO
    {
        public string Name { get; set; }

        public int HostId { get; set; }

        public DateTime DateStart { get; set; } = DateTime.Now;

        public DateTime DateEnd { get; set; } = DateTime.Now.AddHours(2);

        public string Description { get; set; }

        public int? ConferenceRoomId { get; set; }

        public int? ProjectId { get; set; }
        public List<int> Members { get; set; } = new List<int>();
    }
}
