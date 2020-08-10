namespace AccountManager.API.Controllers.V1.DTO
{
    public class LoginResponse: AccountResponse
    {
        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }

        public int ExpiresIn { get; set; }
    }
}
