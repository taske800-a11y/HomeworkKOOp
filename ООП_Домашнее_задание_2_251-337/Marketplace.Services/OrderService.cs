using Marketplace.Domain;

namespace Marketplace.Services;

public static class OrderService
{
    // Одна операция рассчитывает списание денег и бонусов. При отказе исходный баланс не меняется.
    public static bool TryPay(BalanceState balance, PricingResult price, out BalanceState newBalance,
        out decimal bonusPart, out decimal moneyPart, out string message)
    {
        decimal finalPrice = price.GetFinalPrice();
        decimal allowedByShare = decimal.Round(finalPrice * balance.BonusShare, 2, MidpointRounding.AwayFromZero);
        bonusPart = Math.Min(balance.Bonuses, allowedByShare);
        moneyPart = finalPrice - bonusPart;

        if (balance.Money < moneyPart)
        {
            newBalance = balance;
            message = $"Недостаточно средств. Нужно денег: {moneyPart:N2} УЕ, доступно: {balance.Money:N2} УЕ.";
            return false;
        }

        newBalance = balance with
        {
            Money = decimal.Round(balance.Money - moneyPart, 2, MidpointRounding.AwayFromZero),
            Bonuses = decimal.Round(balance.Bonuses - bonusPart, 2, MidpointRounding.AwayFromZero)
        };
        message = "Оплата прошла успешно.";
        return true;
    }
}
