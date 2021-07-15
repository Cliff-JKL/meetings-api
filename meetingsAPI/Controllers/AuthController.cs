using Microsoft.AspNetCore.Mvc;
using meetingsAPI.Models.Authorization;
using meetingsAPI.Services.AuthorizationService;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace meetingsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private static readonly JwtSecurityTokenHandler TokenHandler = new JwtSecurityTokenHandler();

        private IAuthService AuthService { get; }

        public AuthController(IAuthService authService)
        {
            AuthService = authService;
        }

        [HttpPost("token")]
        public async Task<IActionResult> GetToken(AuthFormDTO authForm)
        {
            if (authForm is null)
            {
                throw new ArgumentNullException(nameof(authForm));
            }

            var token = await AuthService.GetTokenAsync(authForm);

            var encodedToken = TokenHandler.WriteToken(token);

            return Ok(new { Token = encodedToken });
        }
    }
}
