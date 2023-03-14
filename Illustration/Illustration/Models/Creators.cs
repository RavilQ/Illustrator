using System.ComponentModel.DataAnnotations;

namespace Illustration.Models
{
    public class Creators
    {
        public int Id { get; set; }
        [MaxLength(120)]
        public string Image { get; set; }
        [MaxLength(20)]
        public string Fullname { get; set; }
        [MaxLength(120)]
        public string Info { get; set; }
    }
}
