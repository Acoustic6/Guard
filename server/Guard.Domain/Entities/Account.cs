using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Guard.Domain.Entities
{
    public class Account
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public virtual ObjectId Id { get; set; }

        public virtual string Login { get; set; }

        public virtual string Password { get; set; }

        [BsonIgnore]
        public virtual string ConfirmationPassword { get; set; }

        public virtual string Role { get; set; }

        [BsonIgnoreIfDefault]
        public virtual ObjectId UserId { get; set; }

        [BsonIgnore]
        public virtual User User { get; set; }
    }
}
