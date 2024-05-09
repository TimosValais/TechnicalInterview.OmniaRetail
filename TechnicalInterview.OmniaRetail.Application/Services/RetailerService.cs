using TechincalInterview.OmniaRetail.Contracts.Adapters;
using TechnicalInterview.OmniaRetail.Application.Models;
using TechnicalInterview.OmniaRetail.Application.Persistence.Repositories;

namespace TechnicalInterview.OmniaRetail.Application.Services
{
    internal class RetailerService : IRetailerService
    {
        private readonly IProductRepository _productRepository;
        private readonly IRetailerProductPriceRepository _retailerProductPriceRepository;
        private readonly ILoggerAdapter<RetailerService> _logger;

        public RetailerService(IProductRepository productRepository,
                                IRetailerProductPriceRepository retailerProductPriceRepository,
                                ILoggerAdapter<RetailerService> logger)
        {
            _productRepository = productRepository;
            _retailerProductPriceRepository = retailerProductPriceRepository;
            _logger = logger;
        }
        public async Task<IEnumerable<Retailer>> GetCompetitorsByProductGroupIdAsync(Guid productGroupId, CancellationToken cancellationToken = default!)
        {
            IEnumerable<Product> groupProducts = await _productRepository.GetProductsByGroupIdAsync(productGroupId, true, cancellationToken);
            IEnumerable<Retailer> retailers = groupProducts
                                            .SelectMany(gp => gp.RetailerProductPrices.Select(rpp => rpp.Retailer))
                                            .DistinctBy(r => r.Id);
            return retailers;
            //IEnumerable<RetailerProductPrice> retailerProductPrices = await _retailerProductPriceRepository.GetAllByProductIdsAsync(groupProducts.Select(p => p.Id));
            //IEnumerable<Retailer> retailers = await _retailerRepository.GetAllByIdsAsync(retailerProductPrices.Select(rpp => rpp.RetailerId));
            //throw new NotImplementedException();
        }

        public async Task<bool> UpdatePricesAsync(IEnumerable<RetailerProductPrice> productRetailerPrices, CancellationToken cancellationToken = default!)
        {
            try
            {
                await _retailerProductPriceRepository.UpdateBatchAsync(productRetailerPrices, cancellationToken);
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
