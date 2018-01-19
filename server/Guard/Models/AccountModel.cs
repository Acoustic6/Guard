using Guard.Domain.Entities;

namespace Guard.Models
{
    public class AccountModel : Account
    {
        public string ConfirmationPassword { get; set; }
    }
}
