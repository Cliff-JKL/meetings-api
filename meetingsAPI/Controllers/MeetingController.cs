using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using meetingsAPI.Models;
using meetingsAPI.Models.MeetingService;
using meetingsAPI.Models.Abstract;
using meetingsAPI.EF;
using meetingsAPI.Services.MeetingService;

namespace meetingsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MeetingController : ControllerBase
    {
        public MeetingController(ILogger<MeetingController> logger, IMeetingContext context, IMeetingService meetingCreator)
        {
            Context = context;
            Logger = logger;
            MeetingService = meetingCreator;
            Logger.LogDebug("injected into meetup controller");
        }

        private ILogger<MeetingController> Logger { get; }
        private IMeetingContext Context { get; }
        private IMeetingService MeetingService { get; }

        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCertainTimesMeetings([FromBody]MeetingTime meetingsTime)
        {
            Logger.LogInformation($"MEETING TIME: {meetingsTime.DateStart} | {meetingsTime.DateEnd}");

            var meetings = await MeetingService.GetCertainTimesMeetingsAsync(meetingsTime);

            return new OkObjectResult(meetings);
            
        }

        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddMeeting(MeetingDTO meeting)
        {
            Logger.LogInformation($"MEETING DATA: {meeting.Name} | {meeting.HostId} | {meeting.ConferenceRoomId}");

            var newMeetingId = await MeetingService.CreateAsync(meeting);

            return new OkObjectResult(new { id = newMeetingId });
        }

        [HttpPut("update/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateMeeting(int id, [FromBody]Mutation<MeetingDTO> meetingMutation)
        {
            Logger.LogInformation($"UPDATE MEETING id{id}");

            var meeting = await MeetingService.UpdateMeetingAsync(id, meetingMutation);

            return new OkObjectResult(meeting);
        }
    }
}
