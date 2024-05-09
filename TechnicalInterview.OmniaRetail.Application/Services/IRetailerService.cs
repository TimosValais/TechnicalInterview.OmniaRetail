using TechnicalInterview.OmniaRetail.Application.Models;

namespace TechnicalInterview.OmniaRetail.Application.Services
{
    public interface IRetailerService
    {
        public Task<bool> UpdatePricesAsync(IEnumerable<RetailerProductPrice> productRetailerPrices, CancellationToken cancellationToken);
        public Task<IEnumerable<Retailer>> GetCompetitorsByProductGroupIdAsync(Guid productGroupId, CancellationToken cancellationToken);
    }
}
