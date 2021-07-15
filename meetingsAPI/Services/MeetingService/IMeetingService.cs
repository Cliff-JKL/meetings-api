using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using meetingsAPI.Models;
using meetingsAPI.Models.MeetingService;
using meetingsAPI.Models.Abstract;

namespace meetingsAPI.Services.MeetingService
{
    public interface IMeetingService
    {
        Task<int?> CreateAsync(MeetingDTO meeting);

        Task<List<ConferenceRoom>> GetFreeConferenceRoomsAsync(Meeting newMeeting);

        Task<List<Meeting>> GetCertainTimesMeetingsAsync(MeetingTime meetingsTime);

        Task<Meeting> UpdateMeetingAsync(int meetingId, Mutation<MeetingDTO> mutation);
    }
}
