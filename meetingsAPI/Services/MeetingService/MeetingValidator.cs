using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using meetingsAPI.Models;

namespace meetingsAPI.Services.MeetingService
{
    public class MeetingValidator : AbstractValidator<Meeting>
    {
        public MeetingValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.HostId).NotEmpty();
            RuleFor(x => x.DateStart).NotEmpty();
            RuleFor(x => x.DateEnd).NotEmpty();
        }
    }
}
