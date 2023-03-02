using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace Illustration.Models
{
    public class OfferPortrait
    {
        public int Id { get; set; }
        public string AppUserId { get; set; }
        public int PortraitId { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal fivePercentPrice { get; set; }

        public AppUser AppUser { get; set; }
        public Portrait Portrait { get; set; }
    }
}
