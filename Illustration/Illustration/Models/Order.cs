using Illustration.Enum;
using System.ComponentModel.DataAnnotations;

namespace Illustration.Models
{
    public class Order
    {
        public int Id { get; set; }
        [MaxLength(70)]
        public string Fullname { get; set; }
        [MaxLength(70)]
        public string Email { get; set; }
        [MaxLength(270)]
        public string? Address { get; set; }
        [MaxLength(170)]
        public string? City { get; set; }
        [MaxLength(570)]
        public string? Note { get; set; }
        public DateTime CreatAt { get; set; } = DateTime.UtcNow.AddHours(4);
        public OrderStatus? Status { get; set; }
        [MaxLength(70)]
        public string? ZipCode { get; set; }
        [MaxLength(70)]
        public string? Company { get; set; }
        [MaxLength(370)]
        public string? AditionalInformation { get; set; }
        public string? AppUserId { get; set; }
        public int? PortraitId { get; set; }

        public AppUser? AppUser { get; set; }
        public Portrait? Portrait { get; set; }

    }
}
