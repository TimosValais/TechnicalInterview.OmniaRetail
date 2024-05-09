using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TechnicalInterview.OmniaRetail.Application.Persistence
{
    internal class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<AppDbContext> optionsBuilder = new();
            optionsBuilder.UseSqlite("Data Source = ProductManagment.db");

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
