using meetingsAPI.Models.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace meetingsAPI.Services.AuthorizationService
{
    public interface IAuthService
    {
        Task<JwtSecurityToken> GetTokenAsync(AuthFormDTO authForm);
    }
}
