namespace Illustration.Models
{
    public class Subscriber
    {
        public int Id { get; set; }
        public string AppUserId { get; set; }
        public string Email { get; set; }

        public AppUser AppUser { get; set; }
    }
}
