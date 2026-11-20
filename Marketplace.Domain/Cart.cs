using System;
using System.Collections.Generic;
using System.Linq;
namespace Marketplace.Domain;

public sealed class Cart
{
    private readonly List<CartItem> _items = new();
    private readonly Dictionary<int, Product> _products = new();
    public int Id { get; }
    public int UserId { get; }
    public IReadOnlyList<CartItem> Items => _items.AsReadOnly();
    public int ItemCount => _items.Sum(item => item.Quantity);
    public Cart(int id, int userId)
    { if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id)); if (userId <= 0) throw new ArgumentOutOfRangeException(nameof(userId)); Id=id; UserId=userId; }
    public void AddItem(Product product, int quantity)
    {
        ArgumentNullException.ThrowIfNull(product);
        if (quantity <= 0) throw new ArgumentOutOfRangeException(nameof(quantity));
        int index = _items.FindIndex(item => item.ProductId == product.Id);
        if (index >= 0) { CartItem old = _items[index]; _items[index] = new CartItem(old.ProductId, checked(old.Quantity + quantity), product.Price); }
        else { _items.Add(new CartItem(product.Id, quantity, product.Price)); }
        _products[product.Id] = product;
    }
    public PricingResult CalculatePrice(decimal personalDiscount)
    {
        if (personalDiscount < PricingResult.MinDiscount || personalDiscount > PricingResult.MaxDiscount) throw new ArgumentOutOfRangeException(nameof(personalDiscount));
        decimal original = 0m;
        foreach (CartItem item in _items) { if (!_products.TryGetValue(item.ProductId, out Product? product)) throw new InvalidOperationException($"Товар {item.ProductId} отсутствует в корзине."); original += (decimal)product.Price * item.Quantity; }
        decimal final = decimal.Round(original * personalDiscount, 2, MidpointRounding.AwayFromZero);
        return new PricingResult(original, personalDiscount, original-final);
    }
    public IReadOnlyList<CartItem> CreatePurchaseSnapshot() => _items.ToList().AsReadOnly();
    public void Clear() { _items.Clear(); _products.Clear(); }
}
