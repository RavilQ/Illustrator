using Illustration.Models;

namespace Illustration.ViewModel
{
    public class ProfileViewModel
    {
        public Portrait Portrait { get; set; }
        public List<Portrait> Portraits { get; set; } = new List<Portrait>();
        public List<WishListItem> WishListItem { get; set; } = new List<WishListItem>();
        public List<Order> MyOrders { get; set; } = new List<Order>();
        public AppUser User { get; set; }
        public AccountDetailViewModel ViewModel { get; set; }
        public List<Order> SaleOrders { get; set; } = new List<Order>();
        public List<ContactMessage> Messages { get; set; } = new List<ContactMessage>();
    }
}
