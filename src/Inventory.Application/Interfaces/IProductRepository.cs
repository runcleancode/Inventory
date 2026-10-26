using Inventory.Application.Queries;
using Inventory.Domain.Entities;

namespace Inventory.Application.Interfaces;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Product> AddAsync(Product product, CancellationToken cancellationToken = default);
    Task UpdateAsync(Product product, CancellationToken cancellationToken = default);
    Task DeleteAsync(Product product, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Product>> GetPagedAsync(ProductQueryParams queryParams, CancellationToken cancellationToken = default);
    Task<int> CountAsync(ProductQueryParams queryParams, CancellationToken cancellationToken = default);
}
