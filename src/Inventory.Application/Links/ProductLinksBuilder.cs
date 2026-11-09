using Inventory.Application.DTOs;

namespace Inventory.Application.Links;

public static class ProductLinksBuilder
{
    public static LinksDto Build(int productId, Func<int, string> productUrl)
    {
        string href = productUrl(productId);

        return new LinksDto(
            Self: new LinkDto(href, "GET"),
            Update: new LinkDto(href, "PUT"),
            Patch: new LinkDto(href, "PATCH"),
            Delete: new LinkDto(href, "DELETE")
        );
    }
}
