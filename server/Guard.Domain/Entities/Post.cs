﻿using System;

namespace Guard.Domain.Entities
{
    public class Post
    {
        public virtual string Id { get; set; }
        public virtual DateTime? CreationDate { get; set; }
        public virtual string Content { get; set; }
        public virtual string OwnerLogin { get; set; }
        public virtual string CreatorLogin { get; set; }
    }
}
