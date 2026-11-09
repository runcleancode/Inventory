using System.Text.Json.Serialization;
using Inventory.Application.DTOs;

namespace Inventory.Application.Common.Models;

public sealed record PagedResult<T>(
    IReadOnlyList<T> Items,
    int Page,
    int PageSize,
    int TotalCount
)
{
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

    public bool HasPrevious => Page > 1;

    public bool HasNext => Page < TotalPages;

    [JsonPropertyName("_links")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public LinksDto? Links { get; init; }
}
