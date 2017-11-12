using System;
using MongoDB.Bson;

namespace Guard.Domain.Entities.Empty
{
    public class EmptyUser : User
    {
        public override ObjectId Id
        {
            get => ObjectId.GenerateNewId();
            set => throw new NotSupportedException();
        }

        public override string FirstName
        {
            get => string.Empty;
            set => throw new NotSupportedException();
        }

        public override string LastName
        {
            get => string.Empty;
            set => throw new NotSupportedException();
        }

        public override string GivenName
        {
            get => string.Empty;
            set => throw new NotSupportedException();
        }

        public override string Email
        {
            get => string.Empty;
            set => throw new NotSupportedException();
        }

        public override DateTime? Birthday
        {
            get => null;
            set => throw new NotSupportedException();
        }
    }
}
