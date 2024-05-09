using Microsoft.EntityFrameworkCore;

namespace TechnicalInterview.OmniaRetail.Application.Persistence
{
    public class DbInitializer : IDbInitializer
    {
        private readonly DbContext _dbContext;

        public DbInitializer(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task InitializeAsync()
        {
            await _dbContext.Database.EnsureCreatedAsync();
        }

        public Task SeedDatabaseAsync()
        {
            throw new NotImplementedException();
        }
    }
}
