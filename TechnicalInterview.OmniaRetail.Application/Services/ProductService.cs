using TechincalInterview.OmniaRetail.Contracts;
using TechincalInterview.OmniaRetail.Contracts.Adapters;
using TechnicalInterview.OmniaRetail.Application.Models;
using TechnicalInterview.OmniaRetail.Application.Persistence.Repositories;

namespace TechnicalInterview.OmniaRetail.Application.Services
{
    internal class ProductService : IProductService
    {
        private readonly ILoggerAdapter<ProductService> _logger;
        private readonly IProductRepository _productRepository;
        private readonly IRetailerProductPriceRepository _retailerProductPriceRepository;

        public ProductService(IProductRepository productRepository, IRetailerProductPriceRepository retailerProductPriceRepository, ILoggerAdapter<ProductService> logger)
        {
            _logger = logger;
            _productRepository = productRepository;
            _retailerProductPriceRepository = retailerProductPriceRepository;
        }
        public async Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            Product? product = await _productRepository.GetByIdAsync(id, cancellationToken);
            return product;
        }

        public async Task<int> GetPriceRecommendationByIdAsync(Guid productId, PriceTier priceTier = default, CancellationToken cancellationToken = default)
        {
            IEnumerable<RetailerProductPrice> retailProductPrices = await _retailerProductPriceRepository.GetAllByProductIdAsync(productId);
            List<int> prices = retailProductPrices.Select(rpp => rpp.Price).ToList();
            int? recommendedPrice = GetRecommendationByTier(prices, priceTier);
            return recommendedPrice ?? 0;
        }

        public async Task<int> GetProductHighestTier1PriceAsync(Guid productId, CancellationToken cancellationToken = default)
        {
            IEnumerable<RetailerProductPrice> allProductRetailerPrices = await _retailerProductPriceRepository.GetAllByProductIdAsync(productId);
            List<int> allPrices = allProductRetailerPrices.Select(prp => prp.Price).ToList();
            return allPrices.Max();
        }

        public async Task<IEnumerable<int>> GetProductPricesAsync(Guid productId, CancellationToken cancellationToken = default)
        {
            IEnumerable<RetailerProductPrice> retailProductPrices = await _retailerProductPriceRepository.GetAllByProductIdAsync(productId);
            List<int> prices = retailProductPrices.Select(rpp => rpp.Price).ToList();
            //ascending order
            prices.Sort();
            return prices;
        }

        private int? GetRecommendationByTier(List<int> prices, PriceTier priceTier)
        {
            List<int> test = [];
            int test2 = test.Min();
            (List<int> Prices, PriceTier PriceTier) tierPrices = SeparatePricesIntoTiers(prices).FirstOrDefault(x => x.PriceTier == priceTier);
            return tierPrices.Prices.Min() - 1;
        }

        private IEnumerable<(List<int> Prices, PriceTier PriceTier)> SeparatePricesIntoTiers(List<int> prices)
        {
            throw new NotImplementedException();
        }
    }
}
