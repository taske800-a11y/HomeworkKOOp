namespace Marketplace.Domain;

public sealed class Product
{
    private readonly int _id;
    private string _name;
    private decimal _unitPrice;

    public int Id => _id;
    public string Name => _name;
    public decimal UnitPrice => _unitPrice;

    public Product(int id, string name, decimal unitPrice)
    {
        ValidateId(id);
        ValidateName(name);
        ValidatePrice(unitPrice);
        _id = id;
        _name = name.Trim();
        _unitPrice = unitPrice;
    }

    public void Rename(string name)
    {
        ValidateName(name);
        _name = name.Trim();
    }

    public void ChangePrice(decimal unitPrice)
    {
        ValidatePrice(unitPrice);
        _unitPrice = unitPrice;
    }

    public override string ToString() => $"{Name} — {UnitPrice:N2} УЕ";

    private static void ValidateId(int id)
    {
        if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id));
    }

    private static void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Название товара не может быть пустым.", nameof(name));
    }

    private static void ValidatePrice(decimal unitPrice)
    {
        if (unitPrice < 0m)
            throw new ArgumentOutOfRangeException(nameof(unitPrice));
    }
}
