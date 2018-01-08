using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Guard.Domain.Entities
{
    public class Post
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }

        public DateTime? CreationDate { get; set; }

        public string Content { get; set; }

        [BsonIgnoreIfDefault]
        public string OwnerLogin { get; set; }

        [BsonIgnoreIfDefault]
        public string CreatorLogin { get; set; }

        [BsonIgnore]
        public User OwnerUser { get; set; }

        [BsonIgnore]
        public User CreatorUser { get; set; }
    }
}
