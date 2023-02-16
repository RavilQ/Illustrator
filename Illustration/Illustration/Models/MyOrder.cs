using Illustration.Enum;

namespace Illustration.Models
{
    public class MyOrder
    {
        public int Id { get; set; }
        public string? AppUserId { get; set; }
        public int PortraitId { get; set; }
        public DateTime CreatAt { get; set; } = DateTime.UtcNow.AddHours(4);
        public OrderStatus Status { get; set; }

        public AppUser? AppUser { get; set; }
        public Portrait Portrait { get; set; }
    }
}
