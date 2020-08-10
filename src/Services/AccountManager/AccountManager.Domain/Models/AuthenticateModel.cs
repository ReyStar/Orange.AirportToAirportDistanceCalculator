namespace AccountManager.Domain.Models
{
    public class AuthenticateModel : Account
    {
        public string AccessToken { get; set; }
        
        public string RefreshToken { get; set; }

        public int ExpiresIn { get; set; }
    }
}
