using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace Guard.Dal
{
    public class MongoDBContext
    {
        IMongoDatabase database;
        IGridFSBucket gridFS;

        public MongoDBContext(string tableName)
        {
            string connectionString = "mongodb://localhost:27017";
            MongoClient client = new MongoClient(connectionString);
            Database = client.GetDatabase(tableName);
        }

        public IMongoDatabase Database { get => database; set => database = value; }
    }
}
