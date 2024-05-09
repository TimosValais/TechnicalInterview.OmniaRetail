using Microsoft.EntityFrameworkCore;
using TechincalInterview.OmniaRetail.Contracts.Adapters;
using TechnicalInterview.OmniaRetail.Application.Models;

namespace TechnicalInterview.OmniaRetail.Application.Persistence.Repositories
{
    internal class RetailerProductPriceRepository : IRetailerProductPriceRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly ILoggerAdapter<RetailerProductPriceRepository> _logger;

        public RetailerProductPriceRepository(AppDbContext dbContext, ILoggerAdapter<RetailerProductPriceRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        public async Task<IEnumerable<RetailerProductPrice>> GetAllByProductIdAsync(Guid productId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.RetailerProductPrices.Where(rpp => rpp.ProductId == productId).ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<RetailerProductPrice>> GetAllByProductIdsAsync(IEnumerable<Guid> productIds, CancellationToken cancellationToken = default)
        {
            return await _dbContext.RetailerProductPrices.Where(rpp => productIds.Contains(rpp.ProductId)).ToListAsync(cancellationToken);
        }

        public async Task<bool> UpdateBatchAsync(IEnumerable<RetailerProductPrice> retailerProductPrices, CancellationToken cancellationToken = default)
        {
            try
            {
                _dbContext.RetailerProductPrices.UpdateRange(retailerProductPrices);

                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return false;
            }


        }
    }
}
