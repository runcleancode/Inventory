using Inventory.Application.Common.Enums;
using Inventory.Application.Interfaces;
using Inventory.Application.Queries;
using Inventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Persistence.Repositories;

internal sealed class ProductRepository(AppDbContext context) : IProductRepository
{
    public async Task<Product> AddAsync(Product product, CancellationToken cancellationToken = default)
    {
        await context.AddAsync(product, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return product;
    }

    public async Task<int> CountAsync(ProductQueryParams queryParams, CancellationToken cancellationToken = default)
    {
        return await ApplyFilters(context.Products.AsNoTracking(), queryParams)
            .CountAsync(cancellationToken);
    }

    public async Task DeleteAsync(Product product, CancellationToken cancellationToken = default)
    {
        context.Products.Remove(product);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await context.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(product => product.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Product>> GetPagedAsync(ProductQueryParams queryParams, CancellationToken cancellationToken = default)
    {
        return await ApplySorting(ApplyFilters(context.Products.AsNoTracking(), queryParams), queryParams)
            .Skip((queryParams.Page - 1) * queryParams.PageSize)
            .Take(queryParams.PageSize)
            .ToListAsync(cancellationToken);
    }


    public async Task UpdateAsync(Product product, CancellationToken cancellationToken = default)
    {
        context.Products.Update(product);
        await context.SaveChangesAsync(cancellationToken);
    }

    private static IQueryable<Product> ApplyFilters(IQueryable<Product> query, ProductQueryParams queryParams)
    {
        if (queryParams.Id.HasValue)
        {
            query = query.Where(product => product.Id == queryParams.Id.Value);
        }

        if (!string.IsNullOrWhiteSpace(queryParams.Name))
        {
            query = query.Where(product => EF.Functions.ILike(product.Name, $"%{queryParams.Name}%"));
        }

        return query;
    }

    private static IQueryable<Product> ApplySorting(IQueryable<Product> query, ProductQueryParams queryParams)
    {
        bool descending = queryParams.SortDir == SortDirection.Desc;

        return queryParams.SortBy?.ToLowerInvariant() switch
        {
            "id" => descending
                ? query.OrderByDescending(product => product.Id)
                : query.OrderBy(product => product.Id),

            "name" => descending
                ? query.OrderByDescending(product => product.Name)
                : query.OrderBy(product => product.Name),

            _ => descending
                ? query.OrderByDescending(product => product.Id)
                : query.OrderBy(product => product.Id),
        };
    }
}
