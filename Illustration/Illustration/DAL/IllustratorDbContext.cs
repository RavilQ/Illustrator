using Illustration.Models;
using Microsoft.EntityFrameworkCore;

namespace Illustration.DAL
{
    public class IllustratorDbContext:DbContext
    {
        public IllustratorDbContext(DbContextOptions<IllustratorDbContext> opt):base(opt)
        {

        }

        public DbSet<Slider> Sliders { get; set; }
    }
}
