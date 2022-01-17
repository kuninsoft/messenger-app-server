using ChatServer.Model;
using Microsoft.EntityFrameworkCore;

namespace ChatServer.DataAccess.EFCore
{
    public sealed class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<Message> Messages { get; set; }
        
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(nameof(User.Username))
                .IsUnique();

            modelBuilder.Entity<Conversation>()
                .HasIndex(nameof(Conversation.Name))
                .IsUnique();
        }
    }
}