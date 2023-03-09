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
        public List<WishListItem> Wishlistitemscount { get; set; } = new List<WishListItem>();
        public List<Portrait> Portraitscount { get; set; } = new List<Portrait>();
        public List<Order> OrderCount { get; set; } = new List<Order>();
        public List<Order> SaleOrderCount { get; set; } = new List<Order>();
    }
}
