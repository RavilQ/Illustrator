using System.ComponentModel.DataAnnotations;

namespace Illustration.Models
{
    public class ContactMessage
    {
        public int Id { get; set; }
        [MaxLength(60)]
        public string? Name { get; set; }
        [MaxLength(800)]
        public string Text { get; set; }
        public string? AppUserId { get; set; }
        public bool IsMember { get; set; }
        public DateTime CreatAt { get; set; } = DateTime.UtcNow.AddHours(4);
        public AppUser? AppUser { get; set; }
    }
}
