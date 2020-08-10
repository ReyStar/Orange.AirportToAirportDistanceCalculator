using System.ComponentModel.DataAnnotations;

namespace AccountManager.API.Controllers.V1.DTO
{
    public class LoginByRefreshTokenRequest
    {
        [Required(AllowEmptyStrings = false)]
        [StringLength(128, MinimumLength = 64)]
        public string RefreshToken { get; set; }
    }
}
