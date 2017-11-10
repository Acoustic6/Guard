using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Guard.Dal;
using Guard.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using Newtonsoft.Json;

namespace Guard.Controllers.Api
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        public static string DbCollectionName = "Accounts";
        private readonly IMongoDbRepository<Account> _accountRepository;
        private readonly IMongoDbRepository<User> _userRepository;

        public AccountController(
            IMongoDbRepository<Account> accountRepository,
            IMongoDbRepository<User> userRepository)
        {
            _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        [Route("Token")]
        public async Task Token()
        {
            var login = Request.Form["login"];
            var password = Request.Form["password"];

            var identity = await GetIdentity(login, password);
            if (identity == null)
            {
                Response.StatusCode = 400;
                await Response.WriteAsync("Invalid username or password.");
                return;
            }

            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                AuthenticationOptions.Issuer,
                AuthenticationOptions.Audience,
                notBefore: now,
                claims: identity.Claims,
                expires: now.Add(TimeSpan.FromMinutes(AuthenticationOptions.Lifetime)),
                signingCredentials: new SigningCredentials(AuthenticationOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
            );
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                token = encodedJwt,
                login = identity.Name
            };
            
            Response.ContentType = "application/json";
            await Response.WriteAsync(JsonConvert.SerializeObject(response, new JsonSerializerSettings { Formatting = Formatting.Indented }));
        }

        [Route("Create")]
        public async Task<IActionResult> Create([FromBody] Account account)
        {
            if (account.Password != account.ConfirmationPassword || account.User == null)
            {
                return new JsonResult(new {success = false});
            }

            account.Role = Role.Default;
            account.Id = ObjectId.GenerateNewId();
            account.User.Id = account.UserId = ObjectId.GenerateNewId();


            await _userRepository.SaveAsync(account.User);
            await _accountRepository.SaveAsync(account);

            return new JsonResult(new { success = true });
        }

        private async Task<ClaimsIdentity> GetIdentity(string login, string password)
        {
            var accounts = await _accountRepository.FilterAsync(x => x.Login == login && x.Password == password);
            var account = accounts.FirstOrDefault();

            if (account == null) return null;

            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, account.Login),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, account.Role)
            };

            var claimsIdentity = new ClaimsIdentity(
                claims,
                "Token",
                ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType
            );

            return claimsIdentity;
        }
    }
}
