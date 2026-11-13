namespace Marketplace.Domain;

public class Discount
{
    private decimal _percent;
    private int _minSum;

    public required string Name { get; init; }

    public decimal Percent
    {
        get => _percent;
        set
        {
            EnsurePercent(value);
            _percent = value;
        }
    }

    public int MinSum
    {
        get => _minSum;
        set
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(value), "Минимальная сумма не может быть отрицательной.");

            _minSum = value;
        }
    }

    public DateTime ValidUntil { get; init; }
    public bool IsActive => ValidUntil > DateTime.Now;

    public int Calculate(int total)
    {
        if (total < 0)
            throw new ArgumentOutOfRangeException(nameof(total), "Сумма заказа не может быть отрицательной.");

        return total < MinSum ? total : ApplyDiscount(total, Percent);
    }

    public int Calculate(int total, decimal percent)
    {
        if (total < 0)
            throw new ArgumentOutOfRangeException(nameof(total), "Сумма заказа не может быть отрицательной.");

        EnsurePercent(percent);
        return total < MinSum ? total : ApplyDiscount(total, percent);
    }

    private static int ApplyDiscount(int total, decimal percent) =>
        decimal.ToInt32(decimal.Round(total * (1m - percent / 100m), MidpointRounding.AwayFromZero));

    private static void EnsurePercent(decimal value)
    {
        if (value is < 0m or > 100m)
            throw new ArgumentOutOfRangeException(nameof(value), "Процент скидки должен быть от 0 до 100.");
    }
}
