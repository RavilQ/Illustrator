using Illustration.Models;

namespace Illustration.ViewModel
{
    public class ShopViewModel
    {
        public List<Portrait> Portraits { get; set; } = new List<Portrait>();
        public List<Category> Categories { get; set; } = new List<Category>();
        public List<Tag> Tags { get; set; } = new List<Tag>();
        public decimal MaxPrice { get; set; }
        public decimal MinPrice { get; set; }
    }
}
