using System.Collections.Generic;
using Guard.Dal;
using Guard.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Guard.Controllers.Api
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        public static string DbCollectionName = "Accounts";
        private readonly IMongoDbRepository<Account> _accountRepository;

        public AccountController(IMongoDbRepository<Account> accountRepository)
        {
            _accountRepository = accountRepository;
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new[] { "AccountController", "Controller" };
        }
        
    }
}
