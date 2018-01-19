﻿using System;

namespace Guard.Domain.Entities
{
    public class User
    {
        public virtual string Id { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string GivenName { get; set; }
        public virtual string Email { get; set; }
        public virtual DateTime? Birthday { get; set; }
    }
}
