using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Guard.Controllers.Api
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        // GET: api/User
        [HttpGet]
        public IEnumerable<string> GetAll()
        {
            return new string[] { "UserController", "Controller" };
        }
    }
}
