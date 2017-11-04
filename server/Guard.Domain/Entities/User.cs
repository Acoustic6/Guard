using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Guard.Domain.Entities
{
    public class User
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string GivenName { get; set; }

        public DateTime? Birthday { get; set; }
    }
}
