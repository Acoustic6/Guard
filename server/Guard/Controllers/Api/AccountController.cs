using Guard.Dal.Services;
using Guard.Domain.Entities;
using Guard.Domain.Entities.MongoDB;
using Guard.Domain.Repositories;
using Guard.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Guard.Controllers.Api
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        public static string DbCollectionName = "Accounts";
        private readonly IMongoDBRepository<MongoDBAccount> _accountRepository;
        private readonly IMongoDBRepository<MongoDBUser> _userRepository;
        private readonly AuthenticationService _authenticationService;

        public AccountController(
            IMongoDBRepository<MongoDBAccount> accountRepository,
            IMongoDBRepository<MongoDBUser> userRepository)
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
        public async Task<IActionResult> Create([FromBody] AccountModel account)
        {
            if (account.Password != account.ConfirmationPassword || account.User == null)
            {
                Response.StatusCode = StatusCodes.Status500InternalServerError;

                return new JsonResult(new RequestResult
                {
                    Status = RequestResultStatus.Error
                });
            }

            var savedAccount = (await _accountRepository.FilterAsync(e => e.Login == account.Login)).FirstOrDefault();
            if (savedAccount != null)
            {
                Response.StatusCode = StatusCodes.Status500InternalServerError;

                return new JsonResult(new RequestResult
                {
                    Status = RequestResultStatus.Error,
                    Message = "Account with the same login exist."
                });
            }

            account.Role = Role.Default;
            account.Id = ObjectId.GenerateNewId().ToString();
            account.User.Id = ObjectId.GenerateNewId().ToString();

            await _userRepository.SaveAsync(new MongoDBUser
            {
                Id = account.User.Id,
                Birthday = account.User.Birthday,
                Email = account.User.Email,
                FirstName = account.User.FirstName,
                GivenName = account.User.GivenName,
                LastName = account.User.LastName
            });

            await _accountRepository.SaveAsync(new MongoDBAccount
            {
                Id = account.Id,
                Login = account.Login,
                Password = account.Password,
                Role = account.Role,
                UserId = account.User.Id
            });

            Response.StatusCode = StatusCodes.Status201Created;

            return new JsonResult(new RequestResult
            {
                Status = RequestResultStatus.Success,
                Message = "Account successfully created."
            });
        }
    }
}
