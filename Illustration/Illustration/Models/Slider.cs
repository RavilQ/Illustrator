using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Illustration.Models
{
    public class Slider
    {
        public int Id { get; set; }
        [MaxLength(70)]
        public string Title { get; set; }
        [MaxLength(150)]
        public string Text { get; set; }
        [MaxLength(200)]
        public string? Image { get; set; }
        public bool IsShow { get; set; }
        [NotMapped]
        public IFormFile? ImageFile { get; set; }
    }
}
