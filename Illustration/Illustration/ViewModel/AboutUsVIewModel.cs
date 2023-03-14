using Illustration.Models;

namespace Illustration.ViewModel
{
    public class AboutUsVIewModel
    {
        public List<Creators> Creators { get; set; } = new List<Creators>();
        public Dictionary<string, string>? Settings { get; set; }
    }
}
