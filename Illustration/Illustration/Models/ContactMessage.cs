using System.ComponentModel.DataAnnotations;

namespace Illustration.Models
{
    public class ContactMessage
    {
        public int Id { get; set; }
        [MaxLength(60)]
        public string? Email { get; set; }
        [MaxLength(800)]
        public string Text { get; set; }
        public string? AppUserId { get; set; }
        public bool? IsMember { get; set; }

        public AppUser? AppUser { get; set; }
    }
}
