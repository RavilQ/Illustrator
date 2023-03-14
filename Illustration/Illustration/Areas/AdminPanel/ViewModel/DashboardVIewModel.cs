using Illustration.Models;

namespace Illustration.Areas.AdminPanel.ViewModel
{
    public class DashboardVIewModel
    {
        public TodoList TodoList { get; set; }
        public List<ContactMessage> Messages { get; set; } = new List<ContactMessage>();
        public List<Portrait> Portraits { get; set; } = new List<Portrait>();
        public List<Order> Orders { get; set; } = new List<Order>();
        public List<Category> Categories { get; set; } = new List<Category>();
        public List<Tag> Tags { get; set; } = new List<Tag>();
    }
}
