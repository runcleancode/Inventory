using System.Text.Json.Serialization;

namespace Inventory.Application.DTOs;

public sealed record LinkDto(string Href, string Method);

public sealed record LinksDto(
    LinkDto Self,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] LinkDto? Update = null,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] LinkDto? Patch = null,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] LinkDto? Delete = null);
