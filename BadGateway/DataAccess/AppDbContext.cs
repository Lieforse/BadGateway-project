using BadGateway.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace BadGateway.DataAccess
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Subscriber> Subscribers { get; set; }
        
        public DbSet<Category> Categories { get; set; }

        public DbSet<Post> Posts { get; set; }

        public DbSet<Feed> Feeds { get; set; }
    }
}
