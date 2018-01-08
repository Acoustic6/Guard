using System;
using System.Threading.Tasks;
using Guard.Dal;
using Guard.Dal.Services;
using Guard.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace Guard.Controllers.Api
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        public static string DbCollectionName = "Accounts";
        private readonly IMongoDbRepository<Account> _accountRepository;
        private readonly IMongoDbRepository<User> _userRepository;
        private readonly AuthenticationService _authenticationService;

        public AccountController(
            IMongoDbRepository<Account> accountRepository,
            IMongoDbRepository<User> userRepository)
        {
            _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _authenticationService = new AuthenticationService(_accountRepository);
        }

        [Route("token")]
        public async Task Token()
        {
            var login = Request.Form["login"];
            var password = Request.Form["password"];

            var token = await _authenticationService.Token(login, password);
            if (token == null)
            {
                Response.StatusCode = 400;
                await Response.WriteAsync("Invalid username or password.");
                return;
            }
            
            Response.ContentType = "application/json";
            await Response.WriteAsync(token);
        }

        [Route("create")]
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
    }
}
