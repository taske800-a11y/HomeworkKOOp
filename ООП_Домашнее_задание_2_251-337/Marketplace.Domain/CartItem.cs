namespace Marketplace.Domain;

// Товарная позиция — отдельный ссылочный объект, хранящий идентификатор товара и количество.
public sealed class CartItem
{
    public int Id { get; }
    public int ProductId { get; }
    public int Quantity { get; }

    public CartItem(int id, int productId, int quantity)
    {
        if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id));
        if (productId <= 0) throw new ArgumentOutOfRangeException(nameof(productId));
        if (quantity <= 0) throw new ArgumentOutOfRangeException(nameof(quantity));
        Id = id;
        ProductId = productId;
        Quantity = quantity;
    }

    public override string ToString() => $"Позиция {Id}: товар ID={ProductId}, количество={Quantity}";
}
