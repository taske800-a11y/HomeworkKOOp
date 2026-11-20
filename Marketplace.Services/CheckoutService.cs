using System;
using System.Collections.Generic;
using System.Linq;
using Marketplace.Domain;

namespace Marketplace.Services;

public class CheckoutService
{
    public int GetTotal(IEnumerable<CartItem> items)
    {
        ArgumentNullException.ThrowIfNull(items);

        int total = 0;

        foreach (CartItem item in items)
            total = checked(total + item.Total);

        return total;
    }

    public IReadOnlyList<CartItem> FindCheaperThan(
        IEnumerable<CartItem> items, int maxPrice)
    {
        ArgumentNullException.ThrowIfNull(items);

        return items
            .Where(item => IsAvailable(item) && item.PricePerUnit < maxPrice)
            .ToList()
            .AsReadOnly();
    }

    public string GetReport(IEnumerable<CartItem> items) =>
        GetReport(items, includeLines: false);

    public string GetReport(IEnumerable<CartItem> items, bool includeLines)
    {
        ArgumentNullException.ThrowIfNull(items);

        List<CartItem> availableItems = items.Where(IsAvailable).ToList();
        int unitsCount = availableItems.Sum(item => item.Quantity);
        int total = GetTotal(availableItems);

        string summary =
            $"Позиций: {availableItems.Count}, единиц товара: {unitsCount}, итого: {PriceFormatter.Format(total)}";

        if (!includeLines || availableItems.Count == 0)
            return summary;

        string[] lines = availableItems.Select(PrepareLine).ToArray();
        return $"{summary}{Environment.NewLine}{string.Join(Environment.NewLine, lines)}";
    }

    private static bool IsAvailable(CartItem item) => item.Quantity > 0;

    private static string PrepareLine(CartItem item) =>
        $"Товар #{item.ProductId}: {PriceFormatter.Format(item.PricePerUnit)} x {item.Quantity} = {PriceFormatter.Format(item.Total)}";
}
