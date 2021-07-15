namespace meetingsAPI.Models
{
    public class UserMeeting
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int MeetingId { get; set; }

        public bool IsTakePart { get; set; }
        public int? Rating { get; set; }
        public string Comment { get; set; }

        public User User { get; set; }
        public Meeting Meeting { get; set; }
    }
}
