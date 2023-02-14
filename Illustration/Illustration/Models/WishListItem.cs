namespace Illustration.Models
{
    public class WishListItem
    {
        public int Id { get; set; }
        public string? AppUserId { get; set; }
        public int PortraitId { get; set; }
        public int Count { get; set; }
        public DateTime CreatAt { get; set; } = DateTime.UtcNow.AddHours(4);

        public AppUser? AppUser { get; set; }
        public Portrait Portrait { get; set; }
    }
}
