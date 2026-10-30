namespace Inventory.Application.Queries;

public static class ProductSortFields
{
    public const string Id = "id";
    public const string Name = "name";

    public static readonly IReadOnlyList<string> All = new[] { Id, Name };
}
