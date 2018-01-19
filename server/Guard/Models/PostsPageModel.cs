using System.Collections.Generic;

namespace Guard.Models
{
    public class PostsPageModel
    {
        public ICollection<PostModel> Posts { get; set; }
        public int CurentPageNumber { get; set; }
        public int MinPageNumber { get; set; }
        public int MaxPageNumber { get; set; }
    }
}
