using Microsoft.EntityFrameworkCore;
using Laboratorium09App.Models;
using Laboratorium09App.Helpers;

namespace Laboratorium09App.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Login> Loginy { get; set; }
        public DbSet<Dane> Dane { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Login>().HasKey(l => l.Id);
            modelBuilder.Entity<Dane>().HasKey(d => d.Id);

            modelBuilder.Entity<Login>().HasData(new Login
            {
                Id = 1,
                Nazwa = "admin",
                HasloHash = HashHelper.ObliczHash("1234")
            });

            modelBuilder.Entity<Dane>().HasData(new Dane
            {
                Id = 1,
                Tresc = "Wpis testowy"
            });
        }
    }
}