﻿using TechnicalInterview.OmniaRetail.Application.Models;

namespace TechnicalInterview.OmniaRetail.Application.Persistence.Repositories
{
    public interface IProductRepository
    {
        public Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        public Task<IEnumerable<Product>> GetAllAsync(CancellationToken cancellationToken = default);
        public Task<IEnumerable<Product>> GetProductsByGroupIdAsync(Guid groupId, CancellationToken cancellationToken = default);

        public Task<IEnumerable<Product>> GetProductsByGroupIdAsync(Guid groupId, bool includeIntermediateTables, CancellationToken cancellationToken = default);


    }
}
