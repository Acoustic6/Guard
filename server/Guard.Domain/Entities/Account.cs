namespace Guard.Domain.Entities
{
    public class Account
    {
        public virtual string Id { get; set; }
        public virtual string Login { get; set; }
        public virtual string Password { get; set; }
        public virtual string Role { get; set; }
        public virtual User User { get; set; }
    }
}
