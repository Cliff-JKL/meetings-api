using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using meetingsAPI.EF;
using meetingsAPI.Models;
using meetingsAPI.Models.Authorization;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace meetingsAPI.Services.AuthorizationService
{
    public class AuthService : IAuthService
    {
        public AuthService(IMeetingContext context)
        {
            Context = context;
        }

        private IMeetingContext Context { get; }

        public async Task<JwtSecurityToken> GetTokenAsync(AuthFormDTO authForm)
        {
            if (authForm is null)
            {
                throw new ArgumentNullException(nameof(authForm));
            }

            var user = await GetUserAsync(authForm);

            var claimsIdentity = GetClaimsIdentity(user);

            var now = DateTime.Now;

            var jwtToken = new JwtSecurityToken(
                AuthOptions.Issuer,
                AuthOptions.Application,
                claimsIdentity.Claims,
                now,
                now.AddHours(AuthOptions.LifetimeInHours),
                new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            return jwtToken;
        }

        private ClaimsIdentity GetClaimsIdentity(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(nameof(AuthUser.Id), nameof(user.Id)),
                new Claim(nameof(AuthUser.Login), user.Login),
                new Claim(nameof(AuthUser.DisplayName), user.Name),
                new Claim(nameof(AuthUser.Email), user.Email),
            };
            var userGroupIds = user.UserRoles.Select(x => new Claim(AuthUser.GroupClaimName, x.RoleId.ToString()));

            claims.AddRange(userGroupIds);
            var claimsIdentity = new ClaimsIdentity(claims);
            return claimsIdentity;
        }

        private async Task<User> GetUserAsync(AuthFormDTO authForm)
        {
            var user = await Context.User
                            .Include(u => u.UserRoles)
                            .Where(u => u.Login == authForm.Login)
                            .AsNoTracking()
                            .FirstOrDefaultAsync();

            if (user is null) throw new Exception($"Пользователь с логином '{authForm.Login}' не найден");

            if (user.Password != authForm.Password) throw new Exception("Неверный пароль");

            return user;
        }
    }
}
