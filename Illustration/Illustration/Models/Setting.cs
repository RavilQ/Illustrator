using System.ComponentModel.DataAnnotations;

namespace Illustration.Models
{
    public class Setting
    {
        public int Id { get; set; }
        [MaxLength(70)]
        public string Key { get; set; }
        [MaxLength(340)]
        public string Value { get; set; }
    }
}
