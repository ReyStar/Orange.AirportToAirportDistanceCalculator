namespace AccountManager.Domain.Models
{
    public class AccountInfo: Account
    {
        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }
    }
}
