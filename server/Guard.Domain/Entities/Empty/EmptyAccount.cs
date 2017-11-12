using System;
using MongoDB.Bson;

namespace Guard.Domain.Entities.Empty
{
    public class EmptyAccount : Account
    {
        public override ObjectId Id {
            get => ObjectId.GenerateNewId();
            set => throw new NotSupportedException();
        }

        public override string Login
        {
            get => string.Empty;
            set => throw new NotSupportedException();
        }

        public override string Password
        {
            get => string.Empty;
            set => throw new NotSupportedException();
        }

        public override string Role
        {
            get => Entities.Role.Default;
            set => throw new NotSupportedException();
        }

        public override ObjectId UserId
        {
            get => ObjectId.Empty;
            set => throw new NotSupportedException();
        }

        public override User User
        {
            get => new EmptyUser();
            set => throw new NotSupportedException();
        }
    }
}
