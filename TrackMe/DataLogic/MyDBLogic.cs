// TrackMe.DAL/MyDbContext.cs
using Microsoft.EntityFrameworkCore;


namespace TrackMe.DataLogic
{
    public class MyDBLogic : DbContext
    {
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("your_sqlite_connection_string_here");
        }
    }

    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        // Додай інші властивості користувача за потребою
    }
}
