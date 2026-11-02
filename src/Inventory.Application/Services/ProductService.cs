using Inventory.Application.Common.Exceptions;
using Inventory.Application.Common.Models;
using Inventory.Application.DTOs;
using Inventory.Application.Extensions;
using Inventory.Application.Interfaces;
using Inventory.Application.Queries;
using Inventory.Domain.Entities;

namespace Inventory.Application.Services;

public sealed class ProductService(
    IProductRepository productRepository,
    IUnitOfWork unitOfWork) : IProductService
{
    public async Task<ProductDto> CreateAsync(CreateProductDto dto, CancellationToken cancellationToken = default)
    {
        Product product = new(dto.Name);

        Product created = await productRepository.AddAsync(product, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return created.ToDto();
    }

    public async Task<ProductDto> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        Product? product = await productRepository.GetByIdAsync(id, cancellationToken);

        if (product is null)
        {
            throw new NotFoundException(nameof(Product), id);
        }

        return product.ToDto();
    }

    public async Task<ProductDto> UpdateAsync(int id, UpdateProductDto dto, CancellationToken cancellationToken = default)
    {
        Product? product = await productRepository.GetByIdAsync(id, cancellationToken);

        if (product is null)
        {
            throw new NotFoundException(nameof(Product), id);
        }

        product.SetName(dto.Name);

        await productRepository.UpdateAsync(product, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return product.ToDto();
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        Product? product = await productRepository.GetByIdAsync(id, cancellationToken);

        if (product is null)
        {
            throw new NotFoundException(nameof(Product), id);
        }

        await productRepository.DeleteAsync(product, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<PagedResult<ProductDto>> GetPagedAsync(ProductQueryParams queryParams, CancellationToken cancellationToken = default)
    {
        ProductQueryParams normalized = queryParams with
        {
            SortBy = queryParams.SortBy?.ToLowerInvariant(),
        };

        IReadOnlyList<Product> products = await productRepository.GetPagedAsync(normalized, cancellationToken);
        int totalCount = await productRepository.CountAsync(normalized, cancellationToken);

        IReadOnlyList<ProductDto> items = products.Select(p => p.ToDto()).ToList();

        return new PagedResult<ProductDto>(items, normalized.Page, normalized.PageSize, totalCount);
    }
}
