using Inventory.Application.Common.Models;
using Inventory.Application.DTOs;
using Inventory.Application.Interfaces;
using Inventory.Domain.Entities;

namespace Inventory.Application.Services;

public sealed class ProductService : IProductService
{
    private const int MaxPageSize = 100;

    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ProductDto> CreateAsync(CreateProductDto dto, CancellationToken cancellationToken = default)
    {
        Product product = new(dto.Name);

        Product created = await _productRepository.AddAsync(product, cancellationToken);

        return MapToDto(created);
    }

    public async Task<ProductDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        Product? product = await _productRepository.GetByIdAsync(id, cancellationToken);

        if (product is null)
        {
            return null;
        }

        return MapToDto(product);
    }

    public async Task<ProductDto?> UpdateAsync(int id, UpdateProductDto dto, CancellationToken cancellationToken = default)
    {
        Product? product = await _productRepository.GetByIdAsync(id, cancellationToken);

        if (product is null)
        {
            return null;
        }

        product.SetName(dto.Name);

        await _productRepository.UpdateAsync(product, cancellationToken);

        return MapToDto(product);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        Product? product = await _productRepository.GetByIdAsync(id, cancellationToken);

        if (product is null)
        {
            return false;
        }

        await _productRepository.DeleteAsync(product, cancellationToken);

        return true;
    }

    public async Task<PagedResult<ProductDto>> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        int normalizedPage = page < 1 ? 1 : page;
        int normalizedPageSize = Math.Clamp(pageSize, 1, MaxPageSize);

        IReadOnlyList<Product> products = await _productRepository.GetPagedAsync(
            normalizedPage, normalizedPageSize, cancellationToken);

        int totalCount = await _productRepository.CountAsync(cancellationToken);

        IReadOnlyList<ProductDto> items = products.Select(MapToDto).ToList();

        return new PagedResult<ProductDto>(items, normalizedPage, normalizedPageSize, totalCount);
    }

    private static ProductDto MapToDto(Product product)
    {
        return new ProductDto(product.Id, product.Name);
    }
}
