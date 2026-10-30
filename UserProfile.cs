using System;
using System.Collections.Generic;

namespace Marketplace.Domain;

public class UserProfile
{
    private double _balance;
    private double _bonusBalance;
    private double _personalDiscount;
    private double _maxBonusShare;

    public User User { get; set; }
    public List<Contact> Contacts { get; set; }
    public List<CartItem> PurchasedItems { get; }

    /// <summary>Баланс в УЕ, не меньше 0.</summary>
    public double Balance
    {
        get => _balance;
        set => _balance = ValidateNonNegative(value, nameof(Balance));
    }

    /// <summary>Баланс в бонусах, не меньше 0.</summary>
    public double BonusBalance
    {
        get => _bonusBalance;
        set => _bonusBalance = ValidateNonNegative(value, nameof(BonusBalance));
    }

    /// <summary>Множитель цены: 0.01-1.00 (1.00 = без скидки).</summary>
    public double PersonalDiscount
    {
        get => _personalDiscount;
        set
        {
            if (double.IsNaN(value) || value < 0.01 || value > 1.00)
                throw new ArgumentOutOfRangeException(nameof(PersonalDiscount), value, "Скидка должна быть в диапазоне 0.01-1.00.");
            _personalDiscount = value;
        }
    }

    /// <summary>Разрешённая доля оплаты бонусами: 0.0-0.5.</summary>
    public double MaxBonusShare
    {
        get => _maxBonusShare;
        set
        {
            if (double.IsNaN(value) || value < 0.0 || value > 0.5)
                throw new ArgumentOutOfRangeException(nameof(MaxBonusShare), value, "Доля бонусов должна быть в диапазоне 0.0-0.5.");
            _maxBonusShare = value;
        }
    }

    public UserProfile(
        User user,
        List<Contact> contacts,
        double balance = 0,
        double bonusBalance = 0,
        double personalDiscount = 1.0,
        double maxBonusShare = 0.0)
    {
        User = user ?? throw new ArgumentNullException(nameof(user));
        Contacts = contacts ?? throw new ArgumentNullException(nameof(contacts));
        PurchasedItems = new List<CartItem>();
        Balance = balance;
        BonusBalance = bonusBalance;
        PersonalDiscount = personalDiscount;
        MaxBonusShare = maxBonusShare;
    }

    private static double ValidateNonNegative(double value, string name)
    {
        if (double.IsNaN(value) || double.IsInfinity(value) || value < 0)
            throw new ArgumentOutOfRangeException(name, value, "Значение не может быть отрицательным.");
        return value;
    }
}