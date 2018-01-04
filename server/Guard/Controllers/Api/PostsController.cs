using Microsoft.AspNetCore.Mvc;

namespace Guard.Controllers.Api
{
    [Route("api/[controller]")]
    public class PostsController : Controller
    {
        [HttpGet]
        [Route("PostsBy/{login}/page/{pageNumber}")]
        public ActionResult GetPostsBy(string login, int? pageNumber)
        {
            return new JsonResult(new { });
        }
    }
}