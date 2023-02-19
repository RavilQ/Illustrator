using Illustration.Models;

namespace Illustration.ViewModel
{
    public class ProfileViewModel
    {
        public Portrait Portrait { get; set; }
        public List<Portrait> Portraits { get; set; } = new List<Portrait>();
        public List<WishListItem> WishListItem { get; set; } = new List<WishListItem>();
        public List<MyOrder> MyOrders { get; set; } = new List<MyOrder>();
        public AppUser User { get; set; }
        public AccountDetailViewModel ViewModel { get; set; }
    }
}
