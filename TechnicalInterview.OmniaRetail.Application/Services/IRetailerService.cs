using TechnicalInterview.OmniaRetail.Application.Models;

namespace TechnicalInterview.OmniaRetail.Application.Services
{
    public interface IRetailerService
    {
        public Task<bool> UpdatePricesAsync(IEnumerable<RetailerProductPrice> productRetailerPrices, CancellationToken cancellationToken = default);
        public Task<IEnumerable<Retailer>> GetCompetitorsByProductGroupIdAsync(Guid productGroupId, Guid retailerId, CancellationToken cancellationToken = default);
    }
}
