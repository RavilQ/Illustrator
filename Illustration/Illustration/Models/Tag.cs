using System.ComponentModel.DataAnnotations;

namespace Illustration.Models
{
    public class Tag
    {
        public int Id { get; set; }
        [MaxLength(70)]
        public string Name { get; set; }
    }
}
