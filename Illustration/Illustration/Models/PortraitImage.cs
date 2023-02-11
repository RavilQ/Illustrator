using System.ComponentModel.DataAnnotations;

namespace Illustration.Models
{
    public class PortraitImage
    {
        public int Id { get; set; }
        [MaxLength(170)]
        public string Image { get; set; }
        public bool ImageStatus { get; set; }
        public int PortraitId { get; set; }

        public Portrait Portrait { get; set; }
    }
}
