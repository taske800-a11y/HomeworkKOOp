namespace Marketplace.Domain;

public readonly struct CartItem
{
    public int ProductId { get; }
    public int Quantity { get; }
    public int PricePerUnit { get; }

    public CartItem(int productId, int quantity, int pricePerUnit)
    {
        if (quantity <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(quantity), quantity,
                "Количество должно быть положительным.");
        }
        if (pricePerUnit < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(pricePerUnit), pricePerUnit,
                "Цена за единицу не может быть отрицательной.");
        }

        ProductId = productId;
        Quantity = quantity;
        PricePerUnit = pricePerUnit;
    }

    public int Total => checked(PricePerUnit * Quantity);
}
