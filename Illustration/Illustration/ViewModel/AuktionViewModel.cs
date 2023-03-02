using Illustration.Models;

namespace Illustration.ViewModel
{
    public class AuktionViewModel
    {
        public Portrait Portrait { get; set; }
        public List<AppUser> AppUsers { get; set; } = new List<AppUser>();
        public List<GroupMessage> GroupMessages { get; set; } = new List<GroupMessage>();
        public OfferPortrait? Offer { get; set; } 
    }
}
