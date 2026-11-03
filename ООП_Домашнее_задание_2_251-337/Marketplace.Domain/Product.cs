namespace Marketplace.Domain;

public sealed class Product
{
    public int Id { get; }
    public string Name { get; }
    public decimal UnitPrice { get; }

    public Product(int id, string name, decimal unitPrice)
    {
        if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id));
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Название товара не может быть пустым.", nameof(name));
        if (unitPrice < 0m) throw new ArgumentOutOfRangeException(nameof(unitPrice));
        Id = id;
        Name = name;
        UnitPrice = unitPrice;
    }

    public override string ToString() => $"{Name} — {UnitPrice:N2} УЕ";
}
