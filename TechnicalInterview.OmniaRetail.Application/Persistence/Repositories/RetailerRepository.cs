namespace TechnicalInterview.OmniaRetail.Application.Persistence.Repositories
{
    internal class RetailerRepository
    {
        private readonly AppDbContext _dbContext;

        public RetailerRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }


    }
}
