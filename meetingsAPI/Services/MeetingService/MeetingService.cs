using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using meetingsAPI.Models;
using meetingsAPI.Models.MeetingService;
using meetingsAPI.Models.Abstract;
using meetingsAPI.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Reflection;
using FluentValidation.AspNetCore;
using System.Linq.Expressions;

namespace meetingsAPI.Services.MeetingService
{
    public class MeetingService : IMeetingService
    {
        private ILogger<MeetingService> Logger { get; }
        private IMeetingContext Context { get; }

        public MeetingService(ILogger<MeetingService> logger, IMeetingContext context)
        {
            Logger = logger;
            Context = context;
        }

        public async Task<int?> CreateAsync(MeetingDTO meetingDTO)
        {
            var meeting = new Meeting
            {
                Name = meetingDTO.Name,
                DateStart = meetingDTO.DateStart,
                DateEnd = meetingDTO.DateEnd,
                Description = meetingDTO.Description,
                HostId = meetingDTO.HostId,
                ConferenceRoomId = meetingDTO.ConferenceRoomId,
                ProjectId = meetingDTO.ProjectId
            };

            // ? theres no need to check room if its a zoom conference
            if (meeting.ConferenceRoomId != null && !await IsRoomFreeAsync(meeting))
            {
                throw new Exception("The chosen room is busy!");
            }        

            await Context.Meeting.AddAsync(meeting);
            await Context.SaveChangesAsync();
            await AddMembersAsync(meetingDTO.Members, meeting);
            
            return meeting.Id;
        }

        public async Task<Meeting> UpdateMeetingAsync(int meetingId, Mutation<MeetingDTO> mutation)
        {
            var exceptions = new List<Exception>();
            var meeting = await Context.Meeting.AsNoTracking().SingleAsync(m => m.Id == meetingId);
            if (meeting == null)
            {
                throw new Exception("Meeting not found!");
            }

            foreach(string f in mutation.Fields)
            {
                string field = f[0].ToString().ToUpper() + f[1..];
                switch(field)
                {
                    case nameof(MeetingDTO.Name):
                        meeting.Name = mutation.Entity.Name;
                        break;
                    case nameof(MeetingDTO.DateStart):
                        meeting.DateStart = mutation.Entity.DateStart;
                        if (mutation.Entity.ConferenceRoomId != null && !await IsRoomFreeAsync(meeting))
                            exceptions.Add(new Exception("The chosen room is busy!"));
                        break;
                    case nameof(MeetingDTO.DateEnd):
                        meeting.DateEnd = mutation.Entity.DateEnd;
                        if (mutation.Entity.ConferenceRoomId != null && !await IsRoomFreeAsync(meeting))
                            exceptions.Add(new Exception("The chosen room is busy!"));
                        break;
                    case nameof(MeetingDTO.Description):
                        meeting.Description = mutation.Entity.Description;
                        break;
                    case nameof(MeetingDTO.ConferenceRoomId):
                        meeting.ConferenceRoomId = mutation.Entity.ConferenceRoomId;
                        if (mutation.Entity.ConferenceRoomId != null && !await IsRoomFreeAsync(meeting))
                            exceptions.Add(new Exception("The chosen room is busy!"));
                        break;
                    case nameof(MeetingDTO.ProjectId):
                        meeting.ProjectId = mutation.Entity.ProjectId;
                        break;
                    case nameof(MeetingDTO.Members):
                        // rewrite
                        //List<int> newMembers = meeting.UserMeetings.Select(um => um.UserId).Intersect(mutation.Entity.Members).Union(mutation.Entity.Members).ToList();
                        // TODO: не работает
                        await DeleteMembersAsync(meeting);
                        await AddMembersAsync(mutation.Entity.Members, meeting);
                        break;
                    default:
                        exceptions.Add(new Exception($"Can't update the field: {field}"));
                        break;
                }
            }

            if (exceptions.Any())
            {
                throw new AggregateException("Multiple Errors Occured", exceptions);
            }

            Context.Meeting.Update(meeting);
            await Context.SaveChangesAsync();
            return meeting;
        }

        private async Task<bool> AddMembersAsync(List<int> members, Meeting meeting)
        {
            // add host (author)
            meeting.UserMeetings.Add(new UserMeeting
            {
                MeetingId = meeting.Id,
                UserId = meeting.HostId,
                IsTakePart = true
            });

            if (members.Any())
            {
                // add members
                members.ForEach(m =>
                {
                    var userMeeting = new UserMeeting
                    {
                        MeetingId = meeting.Id,
                        UserId = m,
                        IsTakePart = false
                    };
                    meeting.UserMeetings.Add(userMeeting);
                });
            }

            await Context.UserMeeting.AddRangeAsync(meeting.UserMeetings);
            await Context.SaveChangesAsync();
            return true;
        }

        private async Task<bool> DeleteMembersAsync(Meeting meeting)
        {
            var userMeetings = await Context.UserMeeting.Where(um => um.MeetingId == meeting.Id).AsNoTracking().ToListAsync();

            if (userMeetings.Any())
            {
                Context.UserMeeting.RemoveRange(userMeetings);
                await Context.SaveChangesAsync();
            }
            return true;
        }

        private async Task<bool> IsRoomFreeAsync(Meeting meeting)
        {
            var roomMeetings = meeting.Id == 0 ?
            Context.Meeting.AsNoTracking() :
            Context.Meeting.Where(m => m.Id != meeting.Id).AsNoTracking() ;

            var sameDayRoomMeetings = await roomMeetings
                .Where(m => m.ConferenceRoomId == meeting.ConferenceRoomId)
                .Where(m => m.DateStart.Day == meeting.DateStart.Day)
                .AsNoTracking()
                .ToListAsync();

            var result = sameDayRoomMeetings
                .Where(m => IsTimeIntersect(m, meeting.DateStart, meeting.DateEnd))
                .ToList();

            return !result.Any();
        }

        public async Task<List<ConferenceRoom>> GetFreeConferenceRoomsAsync(Meeting newMeeting)
        {
            var freeRooms = await Context.ConferenceRoom
                .Where(r => !r.Meetings.AsEnumerable()
                    .Any(
                    m => IsTimeIntersect(m, newMeeting.DateStart, newMeeting.DateEnd)
                    )
                )
                .ToListAsync();

            return freeRooms;
        }

        private bool IsTimeIntersect(Meeting meeting, DateTime dateStart, DateTime dateEnd)
        {
            return
                (meeting.DateStart >= dateStart && meeting.DateEnd <= dateEnd) ||
                (meeting.DateStart < dateStart && meeting.DateEnd > dateEnd) ||
                (meeting.DateStart > dateStart && meeting.DateEnd > dateEnd) ||
                (meeting.DateStart < dateStart && meeting.DateEnd < dateEnd);
        }

        public async Task<List<Meeting>> GetCertainTimesMeetingsAsync(MeetingTime meetingsTime)
        {
            var meetings = await Context.Meeting
                .Where(m => m.DateStart >= meetingsTime.DateStart && m.DateEnd <= meetingsTime.DateEnd)
                .AsNoTracking()
                .ToListAsync();

            if (!meetings.Any())
            {
                throw new Exception("There are no meetings at this time");
            }

            return meetings;
        }
    }
}
