using Illustration.Models;
using System.ComponentModel.DataAnnotations;

namespace Illustration.Areas.AdminPanel.ViewModel
{
    public class AdminResetPasswordViewModel
    {
        [MaxLength(25)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [MaxLength(25)]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password not the same !!")]
        public string ConfirmPassword { get; set; }

        public string user { get; set; }
    }
}
