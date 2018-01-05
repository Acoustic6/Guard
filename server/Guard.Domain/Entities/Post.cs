using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Guard.Domain.Entities
{
    public class Post
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }

        public DateTime CreationDate { get; set; }

        public string Content { get; set; }

        [BsonIgnoreIfDefault]
        public ObjectId AccountId { get; set; }

        [BsonIgnore]
        public Account Account { get; set; }
    }
}
