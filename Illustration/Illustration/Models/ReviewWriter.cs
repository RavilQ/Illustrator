using System.ComponentModel.DataAnnotations;

namespace Illustration.Models
{
    public class ReviewWriter
    {
        public int Id { get; set; }
        [MaxLength(80)]
        public string Fullname { get; set; }
        [MaxLength(100)]
        public string Email { get; set; }
    }
}
