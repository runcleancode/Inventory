using Inventory.Application.DTOs;

namespace Inventory.Application.Interfaces;

public interface IProductService
{
    Task<ProductDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
}
