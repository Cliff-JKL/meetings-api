using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace meetingsAPI.Models.Authorization
{
    public class AuthUser : ClaimsIdentity
    {
        public const string GroupClaimName = "Group";

        public string Id { get; set; }
        public string DisplayName { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }

        public ICollection<int> GroupIds { get; set; } = new List<int>();

        public AuthUser(ClaimsPrincipal claimsPrincipal)
            : base(claimsPrincipal.Claims)
        {
            Id = claimsPrincipal.FindFirstValue(nameof(Id));
            Login = claimsPrincipal.FindFirstValue(nameof(Login));
            DisplayName = claimsPrincipal.FindFirstValue(nameof(DisplayName));
            Email = claimsPrincipal.FindFirstValue(nameof(Email));

            GroupIds = claimsPrincipal.FindAll(GroupClaimName)
                .Select(x => Convert.ToInt32(x.Value))
                .ToList();
        }
    }
}
