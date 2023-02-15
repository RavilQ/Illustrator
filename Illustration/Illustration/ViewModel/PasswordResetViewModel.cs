using System.ComponentModel.DataAnnotations;

namespace Illustration.ViewModel
{
    public class PasswordResetViewModel
    {
        [MaxLength(25)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [MaxLength(25)]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password not the same !!")]
        public string ConfirmPassword { get; set; }
        public string Token { get; set; }
        public string Email { get; set; }
    }
}
