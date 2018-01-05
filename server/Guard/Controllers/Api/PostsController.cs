using Guard.Dal;
using Guard.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Guard.Controllers.Api
{
    [Route("api/[controller]")]
    public class PostsController : Controller
    {
        public static string DbCollectionName = "Posts";
        private readonly IMongoDbRepository<Post> _postRepository;
        private readonly IMongoDbRepository<Account> _accountRepository;
        private readonly IMongoDbRepository<User> _userRepository;

        public PostsController(
            IMongoDbRepository<Post> postRepository,
            IMongoDbRepository<Account> accountRepository,
            IMongoDbRepository<User> userRepository)
        {
            _postRepository = postRepository ?? throw new System.ArgumentNullException(nameof(postRepository));
            _accountRepository = accountRepository ?? throw new System.ArgumentNullException(nameof(accountRepository));
            _userRepository = userRepository ?? throw new System.ArgumentNullException(nameof(userRepository));
        }

        [HttpGet]
        [Route("PostsBy/{login}/page/{pageNumber}")]
        public async Task GetPostsBy(string login, int pageNumber = 0)
        {
            const int batch = 10;

            if (string.IsNullOrWhiteSpace(login))
            {
                throw new ArgumentNullException(nameof(login));
            }

            var account = (await _accountRepository.FilterAsync(e => e.Login == login)).FirstOrDefault();

            if (account == null)
            {
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                await Response.WriteAsync("Account with the same login doesn't exist.");
                return;
            }

            var posts = (await _postRepository.FilterAsync(e => e.AccountId == account.Id));
            var totalPostsCount = posts.Count();
            var maxPageCount = (double) totalPostsCount / batch;
            var floorMaxPageCount = (int) Math.Floor(maxPageCount);
            var maxPage = (maxPageCount - floorMaxPageCount) == 0 ? (floorMaxPageCount > 0 ? floorMaxPageCount - 1 : 0) : floorMaxPageCount;

            if (pageNumber < 0) pageNumber = 0;
            else if (pageNumber > maxPage) pageNumber = maxPage;

            Response.ContentType = "application/json";
            await Response.WriteAsync(JsonConvert.SerializeObject(
                new PostsPage
                {
                    CurentPageNumber = pageNumber,
                    MaxPageNumber = maxPage,
                    MinPageNumber = 0,
                    Posts = posts.Skip(batch * pageNumber).Take(batch).Select(e =>
                        new Post
                        {
                            Account = account,
                            Content = e.Content,
                            CreationDate = e.CreationDate,
                            Id = e.Id
                        })
                    .ToList()
                },
                new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented
                })
            );
        }
    }
}