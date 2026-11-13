using System.Globalization;

namespace Marketplace.Domain;

public sealed class Product
{
    // Закрытые поля: состояние товара нельзя изменить напрямую извне.
    private readonly int id;
    private readonly string title;
    private int price;

    public Product(int id, string title, int price)
    {
        ValidateId(id);
        ValidateTitle(title);
        ValidatePrice(price);

        this.id = id;
        this.title = title.Trim();
        this.price = price;
    }

    public int GetId() => id;

    public string GetTitle() => title;

    public int GetPrice() => price;

    public void SetPrice(int newPrice)
    {
        ValidatePrice(newPrice);
        price = newPrice;
    }

    public string GetInfo()
    {
        return $"{id}. {title} — {price.ToString("N0", CultureInfo.GetCultureInfo("ru-RU"))} у.е.";
    }

    private static void ValidateId(int value)
    {
        if (value <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(value), "ID товара должен быть положительным.");
    }

    private static void ValidateTitle(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException(
                "Название товара не может быть пустым.", nameof(value));
    }

    private static void ValidatePrice(int value)
    {
        if (value < 0)
            throw new ArgumentOutOfRangeException(
                nameof(value), "Цена не может быть отрицательной.");
    }
}
