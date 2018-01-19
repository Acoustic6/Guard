using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Guard.Domain.Entities.MongoDB
{
    public class MongoDBAccount
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public virtual string Id { get; set; }
        public virtual string Login { get; set; }
        public virtual string Password { get; set; }
        public virtual string Role { get; set; }
        [BsonIgnore]
        public virtual User User { get; set; }

        [BsonIgnoreIfDefault]
        public virtual string UserId { get; set; }
    }
}
