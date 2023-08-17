using Microsoft.EntityFrameworkCore;
using ToDoNotes.Models.Domain;

namespace ToDoNotes.Data
{
    public class ToDoNotesDbContext : DbContext
    {
        public ToDoNotesDbContext(DbContextOptions dBContextOptions)
            : base(dBContextOptions)    
        {
        }

        public DbSet<User> User { get; set; }
        public DbSet<Workspace> Workspace { get; set; }
        public DbSet<Note> Note { get; set; }
        public DbSet<ToDo> ToDo { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Workspace>()
                .Property(b => b.CreatedAt)
                .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<Note>()
                .Property(b => b.CreatedAt)
                .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<ToDo>()
                .Property(b => b.CreatedAt)
                .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<User>()
                .Property(b => b.CreatedAt)
                .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Username).IsUnique();
            });
        }
    }
}
