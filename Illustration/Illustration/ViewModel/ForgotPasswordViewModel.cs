using System.ComponentModel.DataAnnotations;

namespace Illustration.ViewModel
{
    public class ForgotPasswordViewModel
    {
        [MaxLength(100)]
        public string Email { get; set; }
    }
}
