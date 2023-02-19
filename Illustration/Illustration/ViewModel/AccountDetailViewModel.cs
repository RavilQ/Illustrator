using System.ComponentModel.DataAnnotations;

namespace Illustration.ViewModel
{
    public class AccountDetailViewModel
    {
        [MaxLength(55)]
        public string Fullname { get; set; }
        [MaxLength(25)]
        public string Username { get; set; }
        [MaxLength(25)]
        public string Email { get; set; }
        [MaxLength(25)]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }
        [MaxLength(25)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [MaxLength(25)]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Password not the same !!")]
        public string ConfirmPassword { get; set; }
        public IFormFile? PosterImage { get; set; }
    }
}
