using System.Collections.Generic;

namespace Guard.Domain.Entities
{
    public class PostsPage
    {
        public ICollection<Post> Posts { get; set; }

        public int CurentPageNumber { get; set; }

        public int MinPageNumber { get; set; }

        public int MaxPageNumber { get; set; }
    }
}
