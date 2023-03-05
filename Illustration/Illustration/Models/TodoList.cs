using System.ComponentModel.DataAnnotations;

namespace Illustration.Models
{
    public class TodoList
    {
        public int Id { get; set; }
        [MaxLength(30)]
        public string Text { get; set; }
    }
}
