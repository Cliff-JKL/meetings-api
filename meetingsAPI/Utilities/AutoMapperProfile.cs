using AutoMapper;
using meetingsAPI.Models;
using meetingsAPI.Models.MeetingService;

namespace meetingsAPI.Utilities
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Meeting, MeetingDTO>();
        }
    }
}