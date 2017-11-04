using System.Collections.Generic;
using Guard.Dal;
using Guard.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Guard.Controllers.Api
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        public static string DbCollectionName = "Users";
        private readonly IMongoDbRepository<User> _userRepository;

        public UserController(IMongoDbRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public IEnumerable<string> GetAll()
        {
            return new[] { "UserController", "Controller" };
        }
    }
}
