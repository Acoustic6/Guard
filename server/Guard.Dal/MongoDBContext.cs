using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace Guard.Dal
{
    public class MongoDbContext
    {
        private const string ConnectionString = "mongodb://localhost:27017/Guard";

        public IMongoDatabase Database { get; }
        public IGridFSBucket GridFs { get; }

        public MongoDbContext()
        {
            var connection = new MongoUrlBuilder(ConnectionString);
            var client = new MongoClient(ConnectionString);

            Database = client.GetDatabase(connection.DatabaseName);
            GridFs = new GridFSBucket(Database);
        }
    }
}
