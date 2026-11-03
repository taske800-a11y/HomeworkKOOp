namespace Marketplace.Domain;

// Неизменяемая структура с результатом расчёта цены.
public readonly record struct PricingResult
{
    public const decimal MinDiscount = 0.01m;
    public const decimal MaxDiscount = 1.00m;

    public decimal OriginalPrice { get; }
    public decimal Discount { get; }
    public decimal DiscountAmount { get; }
    public decimal FinalPrice => OriginalPrice - DiscountAmount;

    public PricingResult(decimal originalPrice, decimal discount, decimal discountAmount)
    {
        if (originalPrice < 0m) throw new ArgumentOutOfRangeException(nameof(originalPrice));
        if (discount < MinDiscount || discount > MaxDiscount) throw new ArgumentOutOfRangeException(nameof(discount));
        if (discountAmount < 0m || discountAmount > originalPrice) throw new ArgumentOutOfRangeException(nameof(discountAmount));
        OriginalPrice = originalPrice;
        Discount = discount;
        DiscountAmount = decimal.Round(discountAmount, 2, MidpointRounding.AwayFromZero);
    }

    public decimal GetFinalPrice() => FinalPrice;
    public decimal GetDiscountAmount() => DiscountAmount;
    public void ShowFinalPrice() => Console.WriteLine($"Итоговая цена: {FinalPrice:N2} УЕ");
    public void ShowDiscountAmount() => Console.WriteLine($"Размер скидки: {DiscountAmount:N2} УЕ");
}
