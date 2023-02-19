using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Illustration.Models
{
    public class AppUser:IdentityUser
    {
        [MaxLength(70)]
        public string Fullname { get; set; }
        [MaxLength(101)]
        public string? Image { get; set; }
        [NotMapped]
        public IFormFile? PosterImage { get; set; } 
    }
}
