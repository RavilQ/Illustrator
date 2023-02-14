using Illustration.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Illustration.DAL
{
    public class IllustratorDbContext:IdentityDbContext
    {
        public IllustratorDbContext(DbContextOptions<IllustratorDbContext> opt):base(opt)
        {

        }

        public DbSet<Slider> Sliders { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Portrait> Portraits { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<PortraitTag> PortraitTags { get; set; }
        public DbSet<PortraitImage> PortraitImages { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<PortraitCategory> PortraitCategories { get; set; }
        public DbSet<ReviewWriter> ReviewWriters { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<WishListItem> WishListItems { get; set; }
    }
}
