using Guard.Domain.Entities.ElasticSearch;
using Nest;
using System.Threading.Tasks;

namespace Guard.Dal
{
    public class PostsElasticSearchRepository
    {
        public ElasticSearchContext ElasticSearchContext { get; }

        public PostsElasticSearchRepository(ElasticSearchContext elasticSearchContext)
        {
            ElasticSearchContext = elasticSearchContext ?? throw new System.ArgumentNullException(nameof(elasticSearchContext));

            var indexDescriptor = new CreateIndexDescriptor(ElasticSearchContext.defaultIndex).Mappings(ms => ms.Map<ElasticSearchPost>(m => m.AutoMap()));
            ElasticSearchContext.ElasticClient.CreateIndex(ElasticSearchContext.defaultIndex, i => indexDescriptor);
        }

        public async Task<IIndexResponse> Index(ElasticSearchPost document)
        {
            return await ElasticSearchContext.ElasticClient.IndexAsync<ElasticSearchPost>(document, i => i.Refresh(Elasticsearch.Net.Refresh.True));
        }

        public async Task<ISearchResponse<ElasticSearchPost>> SearchWithPaginationBy(string ownerLogin, string filter, int pageNumber, int pageSize)
        {
            if (string.IsNullOrWhiteSpace(filter))
            {
                filter = string.Empty;
            }

            return await ElasticSearchContext.ElasticClient
                .SearchAsync<ElasticSearchPost>(x => x
                    .Query(q =>
                        q.Match(m => m.Field(f => f.OwnerLogin).Query(ownerLogin).Strict(true)) &&
                        q.Match(m => m.Query(filter).Field(e => e.Content))
                    )
                    .Sort(s => s.Descending(e => e.CreationDate))
                    .From(pageNumber)
                    .Size(pageSize)
                );
        }
    }
}
