using Guard.Dal;
using Guard.Domain.Entities.ElasticSearch;
using Guard.Domain.Entities.MongoDB;
using Guard.Domain.Repositories;
using Guard.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
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
        private readonly IMongoDBRepository<MongoDBPost> _postRepository;
        private readonly IMongoDBRepository<MongoDBAccount> _accountRepository;
        private readonly IMongoDBRepository<MongoDBUser> _userRepository;
        private readonly PostsElasticSearchRepository _elasticSearchRepository;

        public PostsController(
            IMongoDBRepository<MongoDBPost> postRepository,
            IMongoDBRepository<MongoDBAccount> accountRepository,
            IMongoDBRepository<MongoDBUser> userRepository,
            PostsElasticSearchRepository elasticSearchRepository)
        {
            _postRepository = postRepository ?? throw new ArgumentNullException(nameof(postRepository));
            _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _elasticSearchRepository = elasticSearchRepository ?? throw new ArgumentNullException(nameof(elasticSearchRepository));
        }

        [HttpPut]
        [Authorize]
        public async Task CreatePost()
        {
            var filter = Request.Form["filter"];
            if (!int.TryParse(Request.Form["targetPage"], out int pageNumber))
            {
                pageNumber = 0;
            }
            
            var postString = Request.Form["post"];
            if (string.IsNullOrWhiteSpace(postString))
            {
                throw new ArgumentNullException(nameof(postString));
            }

            var post = JsonConvert.DeserializeObject<PostModel>(postString);

            await CreatePost(post);

            Response.ContentType = "application/json";
            await Response.WriteAsync(JsonConvert.SerializeObject(await GetPosts(pageNumber, post.OwnerLogin, filter)));
        }

        [HttpPost]
        public async Task GetPostsBy()
        {
            var filter = Request.Form["filter"];
            var login = Request.Form["login"];
            if (string.IsNullOrWhiteSpace(login))
            {
                throw new ArgumentNullException(nameof(login));
            }

            if (!int.TryParse(Request.Form["pageNumber"], out int pageNumber))
            {
                pageNumber = 0;
            }

            var account = (await _accountRepository.FilterAsync(e => e.Login == login)).FirstOrDefault();
            if (account == null)
            {
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                await Response.WriteAsync("Account with the same login doesn't exist.");
                return;
            }

            Response.ContentType = "application/json";
            await Response.WriteAsync(JsonConvert.SerializeObject(await GetPosts(pageNumber, account.Login, filter)));
        }

        private async Task CreatePost(PostModel post)
        {
            if (post == null) throw new ArgumentNullException(nameof(post));

            post.Content = post.Content?.Trim();
            if (string.IsNullOrWhiteSpace(post?.Content)) throw new ArgumentNullException(nameof(post.Content));
            if (string.IsNullOrWhiteSpace(post.OwnerLogin)) throw new ArgumentNullException(nameof(post.OwnerLogin));

            post.CreatorLogin = HttpContext?.User?.Identity?.Name;
            if (string.IsNullOrWhiteSpace(post.CreatorLogin)) throw new ArgumentNullException(nameof(post.CreatorLogin));

            var creatorAccount = (await _accountRepository.FilterAsync(e => e.Login == post.CreatorLogin)).FirstOrDefault();
            if (creatorAccount == null)
            {
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                await Response.WriteAsync("Account with the same login doesn't exist.");
                return;
            }

            var ownerAccount = (await _accountRepository.FilterAsync(e => e.Login == post.OwnerLogin)).FirstOrDefault();
            if (ownerAccount == null)
            {
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                await Response.WriteAsync("Account with the same login doesn't exist.");
                return;
            }

            post.CreationDate = DateTime.Now;
            post.Id = ObjectId.GenerateNewId().ToString();

            await _postRepository.SaveAsync(new MongoDBPost
            {
                Id = post.Id,
                Content = post.Content,
                CreationDate = post.CreationDate,
                CreatorLogin = post.CreatorLogin,
                OwnerLogin = post.OwnerLogin
            });

            await _elasticSearchRepository.Index(new ElasticSearchPost
            {
                Id = post.Id,
                Content = post.Content,
                CreationDate = post.CreationDate,
                CreatorLogin = post.CreatorLogin,
                OwnerLogin = post.OwnerLogin
            });
        }

        private async Task<PostsPageModel> GetPosts(int pageNumber, string ownerLogin, string filter = "")
        {
            const int batch = 10;

            var searchResult = (await _elasticSearchRepository.SearchWithPaginationBy(ownerLogin, filter, pageNumber, batch));
            var posts = searchResult.Documents.ToList();
            var totalPostsCount = searchResult.Total;
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

            var targetCreatorLogins = posts.Select(e => e.CreatorLogin).Distinct().ToList();

            var targetCreatorAccounts = await _accountRepository.FilterAsync(e => targetCreatorLogins.Contains(e.Login));
            var targetCreatorUserIds = targetCreatorAccounts.Select(e => e.UserId).ToList();
            var targetCreatorUsers = await _userRepository.FilterAsync(e => targetCreatorUserIds.Contains(e.Id));

            var targetCreatorUsersById = targetCreatorUsers.ToDictionary(e => e.Id);
            var targetCreatorUsersByLogin = targetCreatorAccounts.ToDictionary(e => e.Login, e => targetCreatorUsersById.ContainsKey(e.UserId) ? targetCreatorUsersById[e.UserId] : null);

            return new PostsPageModel
            {
                CurentPageNumber = pageNumber,
                MaxPageNumber = maxPage,
                MinPageNumber = 0,
                Posts = posts
                .Select(e => {
                    var targetCreator = targetCreatorUsersByLogin[e.CreatorLogin];

                    return new PostModel
                    {
                        Id = e.Id,
                        Content = e.Content,
                        CreationDate = e.CreationDate,
                        CreatorLogin = e.CreatorLogin,
                        OwnerLogin = e.OwnerLogin,
                        CreatorUser = new UserModel
                        {
                            FirstName = targetCreator.FirstName,
                            LastName = targetCreator.LastName,
                            GivenName = targetCreator.GivenName,
                            Birthday = targetCreator.Birthday,
                            Email = targetCreator.Email
                        }
                    };
                }).ToList()
            };
        }

    }
}
