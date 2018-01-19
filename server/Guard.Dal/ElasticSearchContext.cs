using Elasticsearch.Net;
using Nest;
using System;

namespace Guard.Dal
{
    public class ElasticSearchContext
    {
        public readonly IElasticClient ElasticClient;
        public readonly string defaultIndex;
        public readonly Uri[] connectionURIs =
        {
            new Uri($"http://localhost:{9200}/")
        };

        public ElasticSearchContext(string index = null)
        {
            defaultIndex = index ?? "default";
            ElasticClient = new ElasticClient(GetConnectionSettings());
        }

        private ConnectionSettings GetConnectionSettings()
        {
            var pool = new StaticConnectionPool(connectionURIs);
            var connSettings = new ConnectionSettings(pool);
            return string.IsNullOrWhiteSpace(defaultIndex)
                ? connSettings
                : connSettings.DefaultIndex(defaultIndex);
        }
    }
}
