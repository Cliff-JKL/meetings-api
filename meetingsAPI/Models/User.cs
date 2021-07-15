using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace meetingsAPI.Models
{
    public class User
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
        public ICollection<UserMeeting> UserMeetings { get; set; } = new List<UserMeeting>();
        public ICollection<Meeting> Meetings { get; set; } = new List<Meeting>();
    }
}
