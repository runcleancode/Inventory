namespace Inventory.Application.DTOs;

public sealed record ProductDto(int Id, string Name)
{
    // Parameterless constructor required by XmlSerializer.
    public ProductDto() : this(0, string.Empty) { }
};
