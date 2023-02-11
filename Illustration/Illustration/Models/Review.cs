using Illustration.Enum;
using System.ComponentModel.DataAnnotations;

namespace Illustration.Models
{
    public class Review
    {
        public int Id { get; set; }
        public int Raiting { get; set; }
        [MaxLength(250)]
        public string Message { get; set; }
        public DateTime CreatAt { get; set; }
        public OrderStatus Status { get; set; }
        public int PortraitId { get; set; }
        public int ReviewWriterId { get; set; }
        public Portrait Portrait { get; set; }
        public ReviewWriter ReviewWriter { get; set; }
    }
}
