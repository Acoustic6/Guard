using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Guard.Domain.Entities
{
    public class User
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public virtual ObjectId Id { get; set; }

        public virtual string FirstName { get; set; }

        public virtual string LastName { get; set; }

        [BsonIgnoreIfNull]
        public virtual string GivenName { get; set; }

        public virtual string Email { get; set; }

        [BsonIgnoreIfNull]
        public virtual DateTime? Birthday { get; set; }
    }
}
