using Inventory.Application.DTOs;
using Inventory.Domain.Entities;

namespace Inventory.Application.Extensions;

public static class ProductMappingExtensions
{
    public static ProductDto ToDto(this Product product)
    {
        return new ProductDto(product.Id, product.Name);
    }
}
