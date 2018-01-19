using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Guard.Domain.Entities.MongoDB
{
    public class MongoDBPost
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public virtual string Id { get; set; }
        public virtual DateTime? CreationDate { get; set; }
        public virtual string Content { get; set; }
        [BsonIgnoreIfDefault]
        public virtual string OwnerLogin { get; set; }
        [BsonIgnoreIfDefault]
        public virtual string CreatorLogin { get; set; }
    }
}
