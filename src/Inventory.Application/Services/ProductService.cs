using Inventory.Application.Common.Exceptions;
using Inventory.Application.Common.Models;
using Inventory.Application.DTOs;
using Inventory.Application.Extensions;
using Inventory.Application.Interfaces;
using Inventory.Application.Queries;
using Inventory.Domain.Entities;

namespace Inventory.Application.Services;

public sealed class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ProductDto> CreateAsync(CreateProductDto dto, CancellationToken cancellationToken = default)
    {
        Product product = new(dto.Name);

        Product created = await _productRepository.AddAsync(product, cancellationToken);

        return created.ToDto();
    }

    public async Task<ProductDto> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        Product? product = await _productRepository.GetByIdAsync(id, cancellationToken);

        if (product is null)
        {
            throw new NotFoundException(nameof(Product), id);
        }

        return product.ToDto();
    }

    public async Task<ProductDto> UpdateAsync(int id, UpdateProductDto dto, CancellationToken cancellationToken = default)
    {
        Product? product = await _productRepository.GetByIdAsync(id, cancellationToken);

        if (product is null)
        {
            throw new NotFoundException(nameof(Product), id);
        }

        product.SetName(dto.Name);

        await _productRepository.UpdateAsync(product, cancellationToken);

        return product.ToDto();
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        Product? product = await _productRepository.GetByIdAsync(id, cancellationToken);

        if (product is null)
        {
            throw new NotFoundException(nameof(Product), id);
        }

        await _productRepository.DeleteAsync(product, cancellationToken);
    }

    public async Task<PagedResult<ProductDto>> GetPagedAsync(ProductQueryParams queryParams, CancellationToken cancellationToken = default)
    {
        IReadOnlyList<Product> products = await _productRepository.GetPagedAsync(queryParams, cancellationToken);

        int totalCount = await _productRepository.CountAsync(queryParams, cancellationToken);

        IReadOnlyList<ProductDto> items = products.Select(p => p.ToDto()).ToList();

        return new PagedResult<ProductDto>(items, queryParams.Page, queryParams.PageSize, totalCount);
    }
}
