using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace meetingsAPI.Models
{
    public class Meeting
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int HostId { get; set; }
        public User Host { get; set; }

        public DateTime DateStart { get; set; }

        public DateTime DateEnd { get; set; }

        public string Description { get; set; }

        public int? ConferenceRoomId { get; set; }
        public ConferenceRoom ConferenceRoom { get; set; }

        public int? ProjectId { get; set; }
        public Project Project { get; set; }

        public ICollection<UserMeeting> UserMeetings { get; set; } = new List<UserMeeting>();
    }
}
