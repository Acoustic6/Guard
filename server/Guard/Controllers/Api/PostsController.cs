using Guard.Dal;
using Guard.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
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

        [HttpPost]
        [Authorize]
        [Route("create")]
        public async Task CreatePost()
        {
            var postString = Request.Form["post"];
            if (string.IsNullOrWhiteSpace(postString))
            {
                throw new ArgumentNullException(nameof(postString));
            }

            var post = JsonConvert.DeserializeObject<Post>(postString);

            if (!int.TryParse(Request.Form["targetPage"], out int pageNumber))
            {
                throw new ArgumentNullException(nameof(pageNumber));
            }

            if (string.IsNullOrWhiteSpace(post?.Content))
            {
                throw new ArgumentNullException(nameof(post.Content));
            }

            post.CreatorLogin = HttpContext.User.Identity.Name;

            if (string.IsNullOrWhiteSpace(post.CreatorLogin))
            {
                throw new ArgumentNullException(nameof(post.CreatorLogin));
            }

            var creatorAccount = (await _accountRepository.FilterAsync(e => e.Login == post.CreatorLogin)).FirstOrDefault();

            if (creatorAccount == null)
            {
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                await Response.WriteAsync("Account with the same login doesn't exist.");
                return;
            }

            post.CreationDate = DateTime.Now;

            await _postRepository.SaveAsync(post);

            Response.ContentType = "application/json";
            await Response.WriteAsync(JsonConvert.SerializeObject(
                await GetPosts(pageNumber, post.OwnerLogin),
                new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented
                })
            );
        }

        [HttpGet]
        [Route("by/{login}/page/{pageNumber}")]
        public async Task GetPostsBy(string login, int pageNumber = 0)
        {
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

            Response.ContentType = "application/json";
            await Response.WriteAsync(JsonConvert.SerializeObject(
                await GetPosts(pageNumber, account.Login),
                new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented
                })
            );
        }

        private async Task<PostsPage> GetPosts(int pageNumber, string ownerLogin)
        {
            const int batch = 10;

            var posts = (await _postRepository.FilterAsync(e => e.OwnerLogin == ownerLogin)).OrderByDescending(e => e.CreationDate.HasValue ? e.CreationDate : DateTime.MinValue);
            var totalPostsCount = posts.Count();
            var maxPageCount = (double)totalPostsCount / batch;
            var floorMaxPageCount = (int)Math.Floor(maxPageCount);
            var maxPage = (maxPageCount - floorMaxPageCount) == 0 ? (floorMaxPageCount > 0 ? floorMaxPageCount - 1 : 0) : floorMaxPageCount;

            if (pageNumber < 0)
            {
                pageNumber = 0;
            }
            else if (pageNumber > maxPage)
            {
                pageNumber = maxPage;
            }

            var targetPosts = posts.Skip(batch * pageNumber).Take(batch).ToList();
            var targetCreatorLogins = targetPosts.Select(e => e.CreatorLogin).Distinct().ToList();

            var targetCreatorAccounts = await _accountRepository.FilterAsync(e => targetCreatorLogins.Contains(e.Login));
            var targetCreatorUserIds = targetCreatorAccounts.Select(e => e.UserId).ToList();
            var targetCreatorUsers = await _userRepository.FilterAsync(e => targetCreatorUserIds.Contains(e.Id));

            var targetCreatorUsersById = targetCreatorUsers.ToDictionary(e => e.Id);
            var targetCreatorUsersByLogin = targetCreatorAccounts.ToDictionary(e => e.Login, e => targetCreatorUsersById.ContainsKey(e.UserId) ? targetCreatorUsersById[e.UserId] : null);

            return new PostsPage
            {
                CurentPageNumber = pageNumber,
                MaxPageNumber = maxPage,
                MinPageNumber = 0,
                Posts = targetPosts
                .Select(e =>
                    new Post
                    {
                        Id = e.Id,
                        Content = e.Content,
                        CreationDate = e.CreationDate,
                        CreatorLogin = e.CreatorLogin,
                        OwnerLogin = e.OwnerLogin,
                        CreatorUser = targetCreatorUsersByLogin[e.CreatorLogin]

                    }).ToList()
            };
        }

    }
}
}