using System.ComponentModel.DataAnnotations;

namespace Illustration.ViewModel
{
    public class MemberLoginViewModel
    {
        [MaxLength(25)]
        public string Email { get; set; }
        [MaxLength(25)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
