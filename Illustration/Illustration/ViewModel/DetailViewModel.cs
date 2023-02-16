using Illustration.Models;

namespace Illustration.ViewModel
{
    public class DetailViewModel
    {
        public Portrait Portrait { get; set; }
        public List<Portrait> Portraits { get; set; } = new List<Portrait>();
        public Review Review { get; set; }
    }
}
