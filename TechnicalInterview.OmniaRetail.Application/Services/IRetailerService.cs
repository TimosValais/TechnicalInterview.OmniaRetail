using TechnicalInterview.OmniaRetail.Application.Domain;

namespace TechnicalInterview.OmniaRetail.Application.Services
{
    public interface IRetailerService
    {
        public Task<bool> UpdatePricesAsync(IEnumerable<ProductRetailerPrice> productRetailerPrices, CancellationToken cancellationToken);
        public Task<IEnumerable<Retailer>> GetCompetitorsByProductGroupIdAsync(Guid productGroupId, CancellationToken cancellationToken);
    }
}
