using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Illustration.Models
{
    public class Portrait
    {
        public int Id { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Weight { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Dimention { get; set; }
        [MaxLength(350)]
        public string Info { get; set; }
        [MaxLength(350)]
        public string Desc { get; set; }
        [MaxLength(150)]
        public string Name { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal SalePrice { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal CostPrice { get; set; }
        public int DiscountPercent { get; set; }
        public bool StockStatus { get; set; } = true;
        public bool IsSpecial { get; set; }
        public bool IsAuktion { get; set; }
        public DateTime CreatAt { get; set; } = DateTime.UtcNow.AddHours(4);
        public string? AppUserId { get; set; }
        [NotMapped]
        public IFormFile? PosterImage { get; set; }
        [NotMapped]
        public List<IFormFile>? OtherImages { get; set; }
        [NotMapped]
        public List<int>? CategoryIds = new List<int>();
        [NotMapped]
        public List<int>? TagIds = new List<int>();

        public List<PortraitImage>? PortraitImages = new List<PortraitImage>();
        public List<PortraitCategory> PortraitCategories = new List<PortraitCategory>();

        public AppUser? AppUser { get; set; }

    }
}
