namespace Marketplace.Domain;

public readonly struct Money
{
    public const decimal MaxAmount = 10_000_000m;

    private const string Rubles = "RUB";

    public static readonly Money Zero = new(0m, Rubles);

    private readonly string? currency;

    public decimal Amount { get; }

    public string Currency => currency ?? Rubles;

    public Money(decimal amount, string currency)
    {
        EnsureAmount(amount);
        EnsureCurrency(currency);

        Amount = amount;
        this.currency = currency.Trim().ToUpperInvariant();
    }

    public static Money FromRubles(decimal amount) => new(amount, Rubles);

    public Money Add(Money other)
    {
        if (Currency != other.Currency)
        {
            throw new InvalidOperationException(
                $"Нельзя сложить суммы в разных валютах: {Currency} и {other.Currency}.");
        }

        return new Money(Amount + other.Amount, Currency);
    }

    public override string ToString() => $"{Amount:N2} {Currency}";

    private static void EnsureAmount(decimal amount)
    {
        if (amount < 0m || amount > MaxAmount)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), amount,
                $"Сумма должна быть от 0 до {MaxAmount:N0}.");
        }
    }

    private static void EnsureCurrency(string currency)
    {
        if (string.IsNullOrWhiteSpace(currency))
        {
            throw new ArgumentException("Валюта не может быть пустой.", nameof(currency));
        }
    }
}
