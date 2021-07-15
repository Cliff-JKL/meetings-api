using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace meetingsAPI.Models.Authorization
{
    public static class AuthOptions
    {
        public const string Issuer = "Simpl.Meetings.Auth";
        public const string Application = "Simpl.Meetings.Backend";
        public const int LifetimeInHours = 24;

        private const string Key = "ImpossibleToHaskAuthKey123@!_bySimpl";

        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key));
        }

    }
}
