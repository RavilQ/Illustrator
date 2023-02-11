using System.ComponentModel.DataAnnotations;

namespace Illustration.Models
{
    public class Category
    {
        public int Id { get; set; }
        [MaxLength(70)]
        public string Name { get; set; }
    }
}
