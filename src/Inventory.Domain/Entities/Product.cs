using Inventory.Domain.Exceptions;

namespace Inventory.Domain.Entities;

public sealed class Product
{
    public const int NameMaxLength = 200;

    public int Id { get; private set; }
    public string Name { get; private set; } = string.Empty;

    // Required by EF Core for materialization (do not use in application code)
    private Product() { }

    public Product(string name)
    {
        SetName(name);
    }

    public void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainException("Product name cannot be empty.");
        }

        string trimmed = name.Trim();

        if (trimmed.Length > NameMaxLength)
        {
            throw new DomainException($"Product name cannot exceed {NameMaxLength} characters.");
        }

        Name = trimmed;
    }
}
