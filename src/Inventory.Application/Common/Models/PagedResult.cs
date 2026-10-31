using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace Inventory.Application.Common.Models;

[XmlRoot("PagedResult")]
public sealed record PagedResult<T>(
    [property: XmlIgnore] IReadOnlyList<T> Items,
    int Page,
    int PageSize,
    int TotalCount
)
{
    // Parameterless constructor required by XmlSerializer.
    public PagedResult() : this(Array.Empty<T>(), 1, 10, 0) { }

    // XmlSerializer cannot serialize interface-typed members; expose a
    // concrete List<T> for XML output only. JsonIgnore keeps the JSON
    // payload unchanged (Items is already serialized under its own name).
    [XmlArray("Items")]
    [XmlArrayItem("Item")]
    [JsonIgnore]
    public List<T> XmlItems => Items.ToList();

    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

    public bool HasPrevious => Page > 1;

    public bool HasNext => Page < TotalPages;
}
