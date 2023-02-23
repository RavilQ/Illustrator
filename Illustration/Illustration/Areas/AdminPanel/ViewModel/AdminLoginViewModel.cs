using System.ComponentModel.DataAnnotations;

namespace Illustration.Areas.AdminPanel.ViewModel
{
    public class AdminLoginViewModel
    {
        [MaxLength(25)]
        public string Username { get; set; }
        [MaxLength(25)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool IsPersist { get; set; }
    }
}
