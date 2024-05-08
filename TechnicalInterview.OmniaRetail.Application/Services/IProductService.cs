using TechincalInterview.OmniaRetail.Contracts;
using TechnicalInterview.OmniaRetail.Application.Domain;

namespace TechnicalInterview.OmniaRetail.Application.Services
{
    public interface IProductService
    {
        public Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        public Task<IEnumerable<int>> GetProductPricesAsync(Guid productId, CancellationToken cancellationToken);

        public Task<int> GetProductHighestTier1PriceAsync(Guid productId, CancellationToken cancellationToken);
        public Task<int> GetPriceRecommendationByIdAsync(Guid productId, PriceTier priceTier, CancellationToken cancellationToken);

    }
}
