using Microsoft.EntityFrameworkCore;
using TechincalInterview.OmniaRetail.Contracts.Adapters;
using TechnicalInterview.OmniaRetail.Application.Models;

namespace TechnicalInterview.OmniaRetail.Application.Persistence.Repositories
{
    internal class ProductGroupRepository : IProductGroupRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly ILoggerAdapter<ProductGroupRepository> _logger;

        public ProductGroupRepository(AppDbContext dbContext, ILoggerAdapter<ProductGroupRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        public async Task<ProductGroup?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.ProductGroups.FirstOrDefaultAsync(pg => pg.Id == id, cancellationToken);
        }

    }
}
