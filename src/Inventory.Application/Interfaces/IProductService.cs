using System.Text.Json;
using Inventory.Application.Common.Models;
using Inventory.Application.DTOs;
using Inventory.Application.Queries;

namespace Inventory.Application.Interfaces;

public interface IProductService
{
    Task<ProductDto> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<ProductDto> CreateAsync(CreateProductDto dto, CancellationToken cancellationToken = default);
    Task<ProductDto> UpdateAsync(int id, UpdateProductDto dto, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<PagedResult<ProductDto>> GetPagedAsync(ProductQueryParams queryParams, CancellationToken cancellationToken = default);
    Task<ProductDto> PatchAsync(int id, JsonElement patchDocument, CancellationToken cancellationToken = default);
}
