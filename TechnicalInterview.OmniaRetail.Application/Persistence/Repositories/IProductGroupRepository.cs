using TechnicalInterview.OmniaRetail.Application.Models;

namespace TechnicalInterview.OmniaRetail.Application.Persistence.Repositories
{
    public interface IProductGroupRepository
    {
        public Task<IEnumerable<ProductGroup>> GetAllAsync(CancellationToken cancellationToken = default);
        public Task<ProductGroup?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    }
}
