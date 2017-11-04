using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Guard.Controllers.Api
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        // GET: api/Account
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "AccountController", "Controller" };
        }
        
    }
}
