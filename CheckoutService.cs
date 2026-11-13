using Marketplace.Domain;

namespace Marketplace.Services;

public class CheckoutService
{
    public int GetTotal(IEnumerable<CartItem> items)
    {
        ArgumentNullException.ThrowIfNull(items);

        int total = 0;
        foreach (CartItem item in items)
        {
            total = checked(total + item.Total);
        }
        return total;
    }

    public IReadOnlyList<CartItem> FindCheaperThan(IEnumerable<CartItem> items, int maxPrice)
    {
        ArgumentNullException.ThrowIfNull(items);

        return items
            .Where(item => IsAvailable(item) && item.PricePerUnit < maxPrice)
            .ToList()
            .AsReadOnly();
    }

    public string GetReport(IEnumerable<CartItem> items) => GetReport(items, false);

    public string GetReport(IEnumerable<CartItem> items, bool includeLines)
    {
        ArgumentNullException.ThrowIfNull(items);

        List<CartItem> available = items.Where(IsAvailable).ToList();
        int units = available.Sum(item => item.Quantity);
        string summary = $"Позиций: {available.Count}, единиц товара: {units}, итого: {GetTotal(available)} у.е.";

        if (!includeLines || available.Count == 0)
        {
            return summary;
        }

        IEnumerable<string> lines = available.Select(PrepareLine);
        return summary + Environment.NewLine + string.Join(Environment.NewLine, lines);
    }

    private bool IsAvailable(CartItem item)
    {
        return item.Quantity > 0;
    }

    private string PrepareLine(CartItem item)
    {
        return $"Товар #{item.ProductId}: {item.PricePerUnit} у.е. x {item.Quantity} = {item.Total} у.е.";
    }
}
