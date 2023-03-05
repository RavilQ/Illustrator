using System.ComponentModel.DataAnnotations;

namespace Illustration.Areas.AdminPanel.ViewModel
{
    public class AdminCreateViewModel
    {
        [MaxLength(55)]
        public string Fullname { get; set; }
        [MaxLength(25)]
        public string Username { get; set; }
        [MaxLength(25)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [MaxLength(25)]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password not the same !!")]
        public string ConfirmPassword { get; set; }
    }
}
