using Marketplace.Domain;

namespace Marketplace.Services;

public static class ReceiptService
{
    public static string BuildReceipt(IEnumerable<CartItem> items)
    {
        ArgumentNullException.ThrowIfNull(items);

        List<CartItem> positions = items.ToList();
        if (positions.Count == 0)
        {
            throw new ArgumentException("Нельзя сформировать чек без позиций.", nameof(items));
        }

        int total = new CheckoutService().GetTotal(positions);
        int totalWithVat = PriceRules.WithVat(total);
        int vatAmount = totalWithVat - total;
        int orderNumber = OrderNumberGenerator.Next();

        List<string> lines = new()
        {
            $"Чек. Заказ № {orderNumber}",
            "----------------------------------------"
        };
        lines.AddRange(positions.Select(PrepareLine));
        lines.Add("----------------------------------------");
        lines.Add($"Итого без НДС: {FormatMoney(total)}");
        lines.Add($"НДС ({PriceRules.VatRate:P0}): {FormatMoney(vatAmount)}");
        lines.Add($"К оплате: {FormatMoney(totalWithVat)}");

        return string.Join(Environment.NewLine, lines);
    }

    private static string PrepareLine(CartItem item)
    {
        return $"Товар #{item.ProductId}: {FormatMoney(item.PricePerUnit)} x {item.Quantity} = {FormatMoney(item.Total)}";
    }

    private static string FormatMoney(int amount)
    {
        return $"{amount:N0} {PriceRules.DefaultCurrency}";
    }
}
