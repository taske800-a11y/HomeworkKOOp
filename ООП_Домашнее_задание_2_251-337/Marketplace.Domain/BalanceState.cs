namespace Marketplace.Domain;

// Значимый тип: копирование и with создают независимое состояние баланса.
public readonly record struct BalanceState
{
    public const decimal MinBonusShare = 0.0m;
    public const decimal MaxBonusShare = 0.5m;

    public decimal Money { get; init; }
    public decimal Bonuses { get; init; }
    public decimal BonusShare { get; init; }

    public BalanceState(decimal money, decimal bonuses, decimal bonusShare)
    {
        if (money < 0m) throw new ArgumentOutOfRangeException(nameof(money), "Денежный баланс не может быть отрицательным.");
        if (bonuses < 0m) throw new ArgumentOutOfRangeException(nameof(bonuses), "Баланс бонусов не может быть отрицательным.");
        if (bonusShare < MinBonusShare || bonusShare > MaxBonusShare)
            throw new ArgumentOutOfRangeException(nameof(bonusShare), "Доля оплаты бонусами должна быть от 0 до 0,5.");
        Money = money;
        Bonuses = bonuses;
        BonusShare = bonusShare;
    }

    public override string ToString() => $"Деньги: {Money:N2} УЕ; бонусы: {Bonuses:N2}; доля бонусов: {BonusShare:P0}";
}
