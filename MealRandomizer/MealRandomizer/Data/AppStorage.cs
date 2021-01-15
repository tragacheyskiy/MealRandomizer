using MealRandomizer.Models;
using MealRandomizer.Services;
using Microsoft.EntityFrameworkCore;
using Xamarin.Forms;

namespace MealRandomizer.Data
{
    internal sealed class AppStorage : DbContext
    {
        private const string DbName = "AppStorage";

        private static readonly string dbPath = DependencyService.Get<IDbPath>().GetDbPath(DbName);

        public DbSet<Product> Products { get; set; }
        public DbSet<Recipe> Recipes { get; set; }

        public AppStorage()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename={dbPath}");
        }
    }
}
