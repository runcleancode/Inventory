namespace Inventory.Domain.Entities;

public class Product
{
    public int Id { get; private set; }
    public string Name { get; private set; } = string.Empty;

    //Required by EF Core for materialization (do not use application code)
    private Product() { }

    public Product(string name)
    {
        SetName(name);
    }

    public void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Product name cannot be empty.", nameof(name));
        }

        Name = name.Trim();
    }
}
