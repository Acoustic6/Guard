using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Guard.Domain.Entities
{
    public class AuthenticationOptions
    {
        public const string Issuer = "TheGuardServer";
        public const string Audience = "GuardProjectAudience";
        public const string Key = "secretkey!SecurityKey!SecurityKey";
        public const int Lifetime = 5;

        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key));
        }
    }
}
