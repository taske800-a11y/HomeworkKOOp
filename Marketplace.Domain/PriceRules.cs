namespace Marketplace.Domain;


public static class PriceRules
{
    public const decimal VatRate = 0.2m;
    public const int MaxDiscountPercent = 70;
    public static readonly string DefaultCurrency = "RUB";

    public static int WithVat(int price)
    {
        EnsurePrice(price);

        decimal withVat = price * (1m + VatRate);
        return decimal.ToInt32(decimal.Round(withVat, MidpointRounding.AwayFromZero));
    }

    public static int ApplyDiscount(int price, int percent)
    {
        EnsurePrice(price);
        EnsurePercent(percent);

        decimal discounted = price * (1m - percent / 100m);
        return decimal.ToInt32(decimal.Round(discounted, MidpointRounding.AwayFromZero));
    }

    private static void EnsurePrice(int price)
    {
        if (price < 0)
            throw new ArgumentOutOfRangeException(nameof(price), "Цена не может быть отрицательной.");
    }

    private static void EnsurePercent(int percent)
    {
        if (percent is < 0 or > MaxDiscountPercent)
            throw new ArgumentOutOfRangeException(
                nameof(percent), $"Процент скидки должен быть от 0 до {MaxDiscountPercent}.");
    }
}