using System.Text.Json.Serialization;

namespace Inventory.Application.DTOs;

public sealed record ProductDto(int Id, string Name)
{
    [JsonPropertyName("_links")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public LinksDto? Links { get; init; }
};
