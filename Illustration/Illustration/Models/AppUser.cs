using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Illustration.Models
{
    public class AppUser:IdentityUser
    {
        [MaxLength(70)]
        public string Fullname { get; set; }
    }
}
