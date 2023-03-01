using System.ComponentModel.DataAnnotations;

namespace Illustration.Models
{
    public class GroupMessage
    {
        public int Id { get; set; }
        [MaxLength(800)]
        public string Text { get; set; }
        public DateTime CreatAt { get; set; } = DateTime.UtcNow.AddHours(4);
        public string? AppUserId { get; set; }
 
        public AppUser? AppUser { get; set; }
    }
}
