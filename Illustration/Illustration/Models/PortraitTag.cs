namespace Illustration.Models
{
    public class PortraitTag
    {
        public int Id { get; set; }
        public int PortraitId { get; set; }
        public int TagId { get; set; }

        public Portrait Portrait { get; set; }
        public Tag Tag { get; set; }
    }
}
