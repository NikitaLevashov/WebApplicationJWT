using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;

namespace WebApplicationJWT
{
    public class AuthOptions
    {
        public const string _issuer = "MyAuthServer";

        public const string _audience = "MyAuthClient";

        const string _key = "mysecret_key!321";

        public const int _lifeTime = 1;
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_key));
        }
    }
}
