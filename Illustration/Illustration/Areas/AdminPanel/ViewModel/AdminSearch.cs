using Illustration.Models;

namespace Illustration.Areas.AdminPanel.ViewModel
{
    public class AdminSearch
    {
        public List<AppUser> Admins { get; set; } = new List<AppUser>();
        public List<Category> Categorys { get; set; } = new List<Category>();
        public List<Portrait> Portraits { get; set; } = new List<Portrait>();
        public List<Slider> Sliders { get; set; } = new List<Slider>();
        public List<Tag> Tags { get; set; } = new List<Tag>();
        public List<AppUser> Users { get; set; } = new List<AppUser>();
    }
}
