namespace Marketplace.Domain;

public sealed class UserProfile
{
    private readonly List<CartItem> _purchases = new();

    public User User { get; }
    public BalanceState Balance { get; private set; }
    // Discount — множитель: 0.95 означает скидку 5%.
    public decimal PersonalDiscount { get; }
    public IReadOnlyList<CartItem> Purchases => _purchases.AsReadOnly();

    public UserProfile(User user, BalanceState balance, decimal personalDiscount)
    {
        ArgumentNullException.ThrowIfNull(user);
        if (personalDiscount < PricingResult.MinDiscount || personalDiscount > PricingResult.MaxDiscount)
            throw new ArgumentOutOfRangeException(nameof(personalDiscount));
        User = user;
        Balance = balance;
        PersonalDiscount = personalDiscount;
    }

    public void UpdateBalance(BalanceState balance) => Balance = balance;

    public void AddPurchases(IEnumerable<CartItem> items)
    {
        ArgumentNullException.ThrowIfNull(items);
        _purchases.AddRange(items.Select(item => new CartItem(item.Id, item.ProductId, item.Quantity)));
    }

    public void Deconstruct(out User user, out int purchasesCount)
    {
        user = User;
        purchasesCount = _purchases.Count;
    }
}
