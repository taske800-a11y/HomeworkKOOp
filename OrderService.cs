using System;
using Marketplace.Domain;

namespace Marketplace.Services;

public class OrderService
{
    public PricingResult Calculate(UserProfile profile, Cart cart)
    {
        ArgumentNullException.ThrowIfNull(profile);
        ArgumentNullException.ThrowIfNull(cart);

        double subtotal = cart.Subtotal;
        double total = Math.Round(subtotal * profile.PersonalDiscount, 2);
        double bonusPart = Math.Round(total * profile.MaxBonusShare, 2);
        double moneyPart = Math.Round(total - bonusPart, 2);

        return new PricingResult(subtotal, total, bonusPart, moneyPart);
    }

    public bool Pay(UserProfile profile, Cart cart)
    {
        ArgumentNullException.ThrowIfNull(profile);
        ArgumentNullException.ThrowIfNull(cart);

        if (cart.IsEmpty)
        {
            Console.WriteLine("Корзина пуста.");
            return false;
        }

        PricingResult price = Calculate(profile, cart);
        BalanceState balance = BalanceState.From(profile);

        if (balance.Money < price.MoneyPart)
        {
            Console.WriteLine($"Недостаточно денег: нужно {price.MoneyPart:F2} УЕ, доступно {balance.Money:F2} УЕ.");
            return false;
        }

        if (balance.Bonuses < price.BonusPart)
        {
            Console.WriteLine($"Недостаточно бонусов: нужно {price.BonusPart:F2}, доступно {balance.Bonuses:F2}.");
            return false;
        }

        BalanceState after = balance.Charge(price);
        profile.Balance = after.Money;
        profile.BonusBalance = after.Bonuses;
        profile.PurchasedItems.AddRange(cart.Items);
        cart.Clear();

        Console.WriteLine($"Оплачено: {price.MoneyPart:F2} УЕ + {price.BonusPart:F2} бонусов.");
        return true;
    }
}