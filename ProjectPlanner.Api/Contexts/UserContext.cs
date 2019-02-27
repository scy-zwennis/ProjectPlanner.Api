using Microsoft.EntityFrameworkCore;
using ProjectPlanner.Api.Models;

namespace ProjectPlanner.Api.Contexts
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>()
                .HasIndex(u => u.EmailAddress)
                .IsUnique();

            builder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            builder.Entity<User>()
                .Property(u => u.FirstName)
                .HasDefaultValue("");

            builder.Entity<User>()
                .Property(u => u.LastName)
                .HasDefaultValue("");
        }

        public DbSet<User> Users { get; set; }
    }
}
