using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WebApplicationJWT.Models;

namespace WebApplicationJWT.Controllers
{
    public class AccountController : Controller
    {
        private List<Person> people = new List<Person>
        {
            new Person {Login="admin@gmail.com", Password = "12345", Role = "admin"},
            new Person {Login="admin1@gmail.com", Password = "123456", Role = "admin"},
            new Person {Login="admin2@gmail.com", Password = "1234567", Role = "admin"},
            new Person {Login="admin3@gmail.com", Password = "12345678", Role = "admin"},
            new Person {Login="qwerty@gmail.com", Password = "4321", Role = "user"},
            new Person {Login="qwerty1@gmail.com", Password = "54321", Role = "user"},
            new Person {Login="qwerty2@gmail.com", Password = "654321", Role = "user"},
            new Person {Login="qwerty3@gmail.com", Password = "7654321", Role = "user"},
            new Person {Login="qwerty4@gmail.com", Password = "87654321", Role = "user"},
        };
        [HttpPost("/token")]
        public IActionResult Token(string userName, string password)
        {
            var identity = GetIdentity(userName, password);

            if (identity == null)
            {
                return BadRequest(new { errorText = "Invalid username or password." });
            }

            var now = DateTime.UtcNow;

            var jwt = new JwtSecurityToken(
                   issuer: AuthOptions._issuer,
                   audience: AuthOptions._audience,
                   notBefore: now,
                   claims: identity.Claims,
                   expires: now.Add(TimeSpan.FromMinutes(AuthOptions._lifeTime)),
                   signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                username = identity.Name
            };

            return Json(response);
        }
        private ClaimsIdentity GetIdentity(string username, string password)
        {
            Person person = people.FirstOrDefault(x => x.Login == username && x.Password == password);
            if (person != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, person.Login),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, person.Role)
                };
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }

            return null;
        }

    }
}