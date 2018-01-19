using Nest;

namespace Guard.Domain.Entities.ElasticSearch
{
    public class ElasticSearchPost : Post
    {
        [Text(Name = "Id")]
        public override string Id { get; set; }
    }
}
