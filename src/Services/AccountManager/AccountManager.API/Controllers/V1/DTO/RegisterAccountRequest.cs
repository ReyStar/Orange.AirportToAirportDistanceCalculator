using System.ComponentModel.DataAnnotations;

namespace AccountManager.API.Controllers.V1.DTO
{
    public class RegisterAccountRequest
    {
        [Required(AllowEmptyStrings = false)]
        [StringLength(256, MinimumLength = 5)]
        public string FirstName { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(256, MinimumLength = 5)]
        public string LastName { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(256, MinimumLength = 5)]
        public string Username { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(256, MinimumLength = 5)]
        public string Password { get; set; }
    }
}
