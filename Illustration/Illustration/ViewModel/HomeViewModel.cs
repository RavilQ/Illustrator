using Illustration.Models;

namespace Illustration.ViewModel
{
    public class HomeViewModel
    {
        public List<Slider> Sliders { get; set; } = new List<Slider>();
        public List<Portrait> Portraits { get; set; } = new List<Portrait>();
        public List<Portrait> SecondPortraits { get; set; } = new List<Portrait>();
        public Dictionary<string, string>? Settings { get; set; }
    }
}
