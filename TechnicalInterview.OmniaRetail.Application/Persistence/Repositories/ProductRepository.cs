using Microsoft.EntityFrameworkCore;
using TechincalInterview.OmniaRetail.Contracts.Adapters;
using TechnicalInterview.OmniaRetail.Application.Models;

namespace TechnicalInterview.OmniaRetail.Application.Persistence.Repositories
{
    internal class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly ILoggerAdapter<ProductRepository> _logger;

        public ProductRepository(AppDbContext dbContext, ILoggerAdapter<ProductRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        public async Task<IEnumerable<Product>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.Products.ToListAsync(cancellationToken);
        }

        public async Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }


        public async Task<IEnumerable<Product>> GetProductsByGroupIdAsync(Guid groupId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Products.Include(p => p.ProductGroups)
                                            .Where(p => p.ProductGroups.Any(pg => pg.Id == groupId))
                                            .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Product>> GetProductsByGroupIdAsync(Guid groupId, bool includeIntermediateTables, CancellationToken cancellationToken = default)
        {
            if (!includeIntermediateTables)
            {
                return await GetProductsByGroupIdAsync(groupId, cancellationToken);
            }
            return await _dbContext.Products.Include(p => p.RetailerProductPrices)
                                .ThenInclude(rpp => rpp.Retailer)
                                .Where(p => p.ProductGroups.Any(pg => pg.Id == groupId))
                                .ToListAsync(cancellationToken);
        }
    }
}
