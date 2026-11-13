
using System;
using System.Collections.Generic;
using System.Linq;

namespace Marketplace.Domain;

public sealed class Cart
{
    private readonly List<CartItem> _items = new();
    private readonly Dictionary<int, Product> _products = new();
    private int _nextItemId = 1;

    public int Id { get; }
    public int UserId { get; }
    public IReadOnlyList<CartItem> Items => _items.AsReadOnly();
    public int ItemCount => _items.Sum(item => item.Quantity);

    public Cart(int id, int userId)
    {
        if (id <= 0)
            throw new ArgumentOutOfRangeException(nameof(id));

        if (userId <= 0)
            throw new ArgumentOutOfRangeException(nameof(userId));

        Id = id;
        UserId = userId;
    }

    public void AddItem(Product product, int quantity)
    {
        ArgumentNullException.ThrowIfNull(product);

        if (quantity <= 0)
            throw new ArgumentOutOfRangeException(nameof(quantity));

        int productId = product.GetId();

        int index = _items.FindIndex(item => item.ProductId == productId);

        if (index >= 0)
        {
            CartItem old = _items[index];

            _items[index] = new CartItem(
                old.Id,
                old.ProductId,
                checked(old.Quantity + quantity));
        }
        else
        {
            _items.Add(
                new CartItem(_nextItemId++, productId, quantity));
        }

        _products[productId] = product;
    }

    public PricingResult CalculatePrice(decimal personalDiscount)
    {
        if (personalDiscount < PricingResult.MinDiscount ||
            personalDiscount > PricingResult.MaxDiscount)
        {
            throw new ArgumentOutOfRangeException(
                nameof(personalDiscount));
        }

        decimal original = 0m;

        foreach (CartItem item in _items)
        {
            if (!_products.TryGetValue(
                    item.ProductId, out Product? product))
            {
                throw new InvalidOperationException(
                    $"Товар {item.ProductId} отсутствует в каталоге корзины.");
            }

            original += (decimal)product.GetPrice() * item.Quantity;
        }

        decimal discountAmount = decimal.Round(
            original * (1m - personalDiscount),
            2,
            MidpointRounding.AwayFromZero);

        return new PricingResult(
            original,
            personalDiscount,
            discountAmount);
    }

    public IReadOnlyList<CartItem> CreatePurchaseSnapshot() =>
        _items
            .Select(item => new CartItem(
                item.Id,
                item.ProductId,
                item.Quantity))
            .ToList()
            .AsReadOnly();

    public void Clear()
    {
        _items.Clear();
        _products.Clear();
    }
}
