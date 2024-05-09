using TechnicalInterview.OmniaRetail.Application.Models;

namespace TechnicalInterview.OmniaRetail.Application.Persistence.Repositories
{
    internal interface IProductGroupRepository
    {
        public Task<ProductGroup> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        public Task<IEnumerable<Product>> GetProductsByGroupIdAsync(Guid groupId, CancellationToken cancellationToken = default);

        public Task<IEnumerable<Retailer>> GetRetailersByGroupAsync(Guid groupId, CancellationToken cancellationToken = default);
    }
}
