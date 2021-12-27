using ChatServer.Model;
using Microsoft.EntityFrameworkCore;

namespace ChatServer.DataAccess
{
    public class AppContext : DbContext
    {
        private const string ConnectionString = @"Server=(localdb)\mssqllocaldb;Database=ChatApp;Trusted_Connection=True";
        
        public DbSet<User> Users { get; set; }
        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<Message> Messages { get; set; }
        
        public AppContext()
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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConnectionString);
        }
    }
}