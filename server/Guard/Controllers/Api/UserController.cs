using Guard.Domain.Entities.MongoDB;
using Guard.Domain.Repositories;
using Guard.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Guard.Controllers.Api
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        public static string DbCollectionName = "Users";
        private readonly IMongoDBRepository<MongoDBUser> _userRepository;
        private readonly IMongoDBRepository<MongoDBAccount> _accountRepository;

        public UserController(
            IMongoDBRepository<MongoDBUser> userRepository,
            IMongoDBRepository<MongoDBAccount> accountRepository)
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
                new UserModel
                {
                    Birthday = user.Birthday,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    GivenName = user.GivenName,
                    LastName = user.LastName
                })
            );
        }
    }
}
