using Microsoft.EntityFrameworkCore;
using UsersAPI.Models;

namespace UsersAPI
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<UserState> UserStates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOne(u => u.UserGroup)
                .WithMany()
                .HasForeignKey(u => u.UserGroupId);

            modelBuilder.Entity<User>()
                .HasOne(u => u.UserState)
                .WithMany()
                .HasForeignKey(u => u.UserStateId);
        }
    }
}
