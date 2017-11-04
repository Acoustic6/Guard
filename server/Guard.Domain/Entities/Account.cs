using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Guard.Domain.Entities
{
    public class Account
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public User User { get; set; }
    }
}
