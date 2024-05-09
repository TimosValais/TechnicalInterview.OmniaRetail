using TechnicalInterview.OmniaRetail.Application.Models;

namespace TechnicalInterview.OmniaRetail.Application.Persistence.Repositories
{
    internal interface IRetailerProductPriceRepository
    {
        public Task<bool> UpdateBatchAsync(IEnumerable<RetailerProductPrice> retailerProductPrices, CancellationToken cancellationToken = default);

        public Task<IEnumerable<RetailerProductPrice>> GetAllByProductIdAsync(Guid productId);

        public Task<IEnumerable<RetailerProductPrice>> GetAllByProductIdsAsync(IEnumerable<Guid> productIds);
    }
}
