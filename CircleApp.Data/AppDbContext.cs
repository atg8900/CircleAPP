using CircleApp.Data.Models;
using CircleAPP.Models;
using Microsoft.EntityFrameworkCore;

namespace CircleAPP.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Post> Posts { get; set; }
        public DbSet<User> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>()
                .HasMany(u=>u.Posts)
                .WithOne(p=>p.User)
                .HasForeignKey(p=>p.UserId);
               
        }
    }
}
   