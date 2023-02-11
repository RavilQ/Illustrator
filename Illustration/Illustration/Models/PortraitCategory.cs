namespace Illustration.Models
{
    public class PortraitCategory
    {
        public int Id { get; set; }
        public int PortraitId { get; set; }
        public int CategoryId { get; set; }

        public Portrait Portrait { get; set; }
        public Category Category { get; set; }
    }
}
