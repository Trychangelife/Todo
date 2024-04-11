using Microsoft.EntityFrameworkCore;
using Todo.Models.Entities;

namespace Todo.Models

{
    public class TodoListContext : DbContext
    {
        public DbSet<UserEntity> Users { get; set; }
        
        public DbSet<TaskApp> Tasks { get; set; }


        public TodoListContext(DbContextOptions options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserEntity>()
                .HasIndex(u => u.Login)
                .IsUnique();

            modelBuilder.Entity<UserEntity>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }
    }
}