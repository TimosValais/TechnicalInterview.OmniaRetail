using TechincalInterview.OmniaRetail.Contracts;
using TechnicalInterview.OmniaRetail.Application.Models;

namespace TechnicalInterview.OmniaRetail.Application.Services
{
    public interface IProductService
    {
        public Task<IEnumerable<Product>> ListAsync(CancellationToken cancellationToken = default);
        public Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        public Task<IEnumerable<int>> GetProductPricesAsync(Guid productId, CancellationToken cancellationToken = default);

        public Task<int> GetProductHighestTier1PriceAsync(Guid productId, CancellationToken cancellationToken = default);
        public Task<int> GetPriceRecommendationByIdAsync(Guid productId, PriceTier priceTier = default, CancellationToken cancellationToken = default);

        public Task<IEnumerable<ProductGroup>> ListProductGroupsAsync(CancellationToken cancelationToken = default!);
    }
}
