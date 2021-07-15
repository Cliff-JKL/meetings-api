using Microsoft.AspNetCore.Http;
using meetingsAPI.Models.Authorization;
using System;
using System.Linq;

namespace meetingsAPI.Extensions
{
    public static class HttpContextExtension
    {
        public static AuthUser GetUserInfo(this HttpContext context)
        {
            return context.User.Identities.SingleOrDefault(i => i.GetType() == typeof(AuthUser)) as AuthUser
                ?? throw new UnauthorizedAccessException();
        }
    }
}
