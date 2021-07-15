using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using meetingsAPI.Extensions;
using meetingsAPI.Models.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace meetingsAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpGet("info")]
        public ActionResult<AuthUser> GetInfo()
        {
            var userInfo = HttpContext.GetUserInfo();

            return Ok(new 
            {
                userInfo.Id,
                userInfo.Login,
                userInfo.DisplayName,
                userInfo.Email,
                userInfo.GroupIds
            });
        }
    }
}
