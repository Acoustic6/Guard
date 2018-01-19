using Guard.Domain.Entities;

namespace Guard.Models
{
    public class PostModel : Post
    {
        public UserModel CreatorUser { get; set; }
    }
}
