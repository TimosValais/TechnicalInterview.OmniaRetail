using TechincalInterview.OmniaRetail.Contracts;
using TechincalInterview.OmniaRetail.Contracts.Adapters;
using TechnicalInterview.OmniaRetail.Application.Extensions;
using TechnicalInterview.OmniaRetail.Application.Models;
using TechnicalInterview.OmniaRetail.Application.Persistence.Repositories;

namespace TechnicalInterview.OmniaRetail.Application.Services
{
    internal class ProductService : IProductService
    {
        private readonly ILoggerAdapter<ProductService> _logger;
        private readonly IProductRepository _productRepository;
        private readonly IRetailerProductPriceRepository _retailerProductPriceRepository;
        private readonly IProductGroupRepository _productGroupRepository;

        public ProductService(IProductRepository productRepository,
                                IRetailerProductPriceRepository retailerProductPriceRepository,
                                IProductGroupRepository productGroupRepository,
                                ILoggerAdapter<ProductService> logger)
        {
            _logger = logger;
            _productRepository = productRepository;
            _retailerProductPriceRepository = retailerProductPriceRepository;
            _productGroupRepository = productGroupRepository;
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

        public async Task<IEnumerable<Product>> ListAsync(CancellationToken cancellationToken = default)
        {
            return await _productRepository.GetAllAsync();
        }


        public async Task<IEnumerable<ProductGroup>> ListProductGroupsAsync(CancellationToken cancelationToken = default)
        {
            return await _productGroupRepository.GetAllAsync(cancelationToken);
        }

        private int? GetRecommendationByTier(List<int> prices, PriceTier priceTier)
        {
            //Get the prices of the requested price Tier
            (List<int> Prices, PriceTier PriceTier) tierPrices = SeparatePricesIntoTiers(prices).FirstOrDefault(x => x.PriceTier == priceTier);
            //in case there are no prices in the selected tier, we need handling
            if (tierPrices.Prices.Count == 0)
            {
                return FindNextBestRecommendation(tierPrices, priceTier, prices);
            }
            //recommend a price 1 cent less than the lowest price of the requested tier
            return tierPrices.Prices.Min() - 1;
        }

        private int FindNextBestRecommendation((List<int> Prices, PriceTier PriceTier) tierPrices, PriceTier priceTier, List<int> prices)
        {
            int? resultPrice = null;
            IEnumerable<(List<int> Prices, PriceTier PriceTier)> tieredPrices = SeparatePricesIntoTiers(prices);
            (List<int> Prices, PriceTier PriceTier) tier1Prices = tieredPrices.FirstOrDefault(tp => tp.PriceTier == PriceTier.Tier1);
            (List<int> Prices, PriceTier PriceTier) tier2Prices = tieredPrices.FirstOrDefault(tp => tp.PriceTier == PriceTier.Tier2);
            (List<int> Prices, PriceTier PriceTier) tier3Prices = tieredPrices.FirstOrDefault(tp => tp.PriceTier == PriceTier.Tier3);
            switch (priceTier)
            {
                //If they are looking for the most expensive tier, 1 cent more than the most expensive of the next tier will do
                case PriceTier.Tier1:
                    if (tier2Prices.Prices.Count != 0)
                    {
                        resultPrice = tier2Prices.Prices.Max() + 1;
                    }
                    else if (tier3Prices.Prices.Any())
                    {
                        resultPrice = tier3Prices.Prices.Max() + 1;
                    }
                    break;
                //If they are looking for the average tier and nothing is found there (highly unlikely), first check for higher tier and then for lower
                case PriceTier.Tier2:
                    if (tier3Prices.Prices.Count != 0)
                    {
                        resultPrice = tier3Prices.Prices.Min() - 1;
                    }
                    else if (tier1Prices.Prices.Count != 0)
                    {
                        resultPrice = tier1Prices.Prices.Max() + 1;
                    }
                    break;
                //if they are looking for the best cheapest price, then anything cheaper than the next available will do
                case PriceTier.Tier3:
                    if (tier2Prices.Prices.Count != 0)
                    {
                        resultPrice = tier2Prices.Prices.Min() - 1;
                    }
                    else if (tier3Prices.Prices.Count != 0)
                    {
                        resultPrice = tier3Prices.Prices.Min() - 1;
                    }
                    break;
                default:
                    break;
            }
            if (resultPrice is null)
            {
                _logger.LogInformation("No result price was found");
                throw new InvalidDataException("No prices where found to recommend");
            }
            //this will never be null though
            return resultPrice ?? 0;
        }

        /// <summary>
        /// This is based on Stardard deviation : https://en.wikipedia.org/wiki/Standard_deviation
        /// We are basically declaring Tier 1 the highest prices (over 1 sigma), Tier 3 the lowest (under 1 sigma)
        /// and Tier 2 are in between, 1 sigma around the mean.
        /// </summary>
        /// <param name="prices"></param>
        /// <returns>A tuple with the Prices per PriceTier</returns>
        /// <exception cref="NotImplementedException"></exception>
        private IEnumerable<(List<int> Prices, PriceTier PriceTier)> SeparatePricesIntoTiers(List<int> prices)
        {
            double average = prices.Average();
            double sigma = prices.StandardDeviation();
            List<(List<int> Prices, PriceTier PriceTier)> tieredPrices =
            [
                new (prices.Where(p => p > average + sigma).ToList(), PriceTier.Tier1),
                new (prices.Where(p => p >= average - sigma && p <= average + sigma).ToList(), PriceTier.Tier2),
                new (prices.Where(p => p < average - sigma).ToList(), PriceTier.Tier3),
            ];
            return tieredPrices;
        }

    }
}
