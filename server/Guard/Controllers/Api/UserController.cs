using System;
using System.Linq;
using System.Threading.Tasks;
using Guard.Dal;
using Guard.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using JsonConvert = Newtonsoft.Json.JsonConvert;

namespace Guard.Controllers.Api
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        public static string DbCollectionName = "Users";
        private readonly IMongoDbRepository<User> _userRepository;
        private readonly IMongoDbRepository<Account> _accountRepository;

        public UserController(
            IMongoDbRepository<User> userRepository,
            IMongoDbRepository<Account> accountRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
        }

        [HttpGet]
        [Route("by/{login}")]
        public async Task GetUserByLogin(string login)
        {
            if (login == null) throw new ArgumentNullException(nameof(login));

            var account = (await _accountRepository.FilterAsync(e => e.Login == login)).FirstOrDefault();
            if (account == null)
            {
                Response.StatusCode = 400;
                await Response.WriteAsync("Account with the same login doesn't exist.");
                return;
            }

            var user = (await _userRepository.FilterAsync(e => e.Id == account.UserId)).FirstOrDefault();

            if (user == null)
            {
                Response.StatusCode = 400;
                await Response.WriteAsync("User with the same login doesn't exist.");
                return;
            }

            Response.ContentType = "application/json";
            await Response.WriteAsync(JsonConvert.SerializeObject(
                new
                {
                    user.Birthday,
                    user.Email,
                    user.FirstName,
                    user.GivenName,
                    user.LastName
                },
                new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented
                })
            );
        }
    }
}
