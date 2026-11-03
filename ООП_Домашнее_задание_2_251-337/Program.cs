using Marketplace.Domain;
using Marketplace.Services;

Console.OutputEncoding = System.Text.Encoding.UTF8;

Console.WriteLine("=== ООП. Домашнее задание №2: интернет-магазин ===");
var user = new User(1, "Лохмаев Алексей Сергеевич", "aleksei");

// Шаги 1–4 сценария: пополнение, начисление бонусов и настройка условий.
decimal topUp = 10_000m;
decimal giftedBonuses = topUp * 0.10m;
var balance = new BalanceState(topUp, giftedBonuses, 0.10m);
var profile = new UserProfile(user, balance, 0.95m);

Console.WriteLine($"Покупатель: {profile.User.FullName}");
Console.WriteLine($"После пополнения и начисления бонусов: {profile.Balance}");
Console.WriteLine("Персональная скидка: 5%; разрешённая доля оплаты бонусами: 10%.");

// Демонстрация ссылочного типа: две переменные ссылаются на один профиль.
UserProfile sameProfile = profile;
sameProfile.UpdateBalance(profile.Balance with { BonusShare = 0.10m });
Console.WriteLine($"Ссылки на один профиль: {ReferenceEquals(profile, sameProfile)}");

// Демонстрация копирования значимого типа: исходный баланс не меняется.
BalanceState balanceCopy = profile.Balance;
BalanceState changedCopy = balanceCopy with { Money = balanceCopy.Money - 100m };
Console.WriteLine($"Копия структуры после with: {changedCopy}");
Console.WriteLine($"Исходная структура не изменилась: {profile.Balance}");

// Каталог товаров.
var plate = new Product(1, "Тарелка", 1_000m);
var cutlery = new Product(2, "Приборы", 2_000m);

// Заказ №1: 3 тарелки и 2 комплекта приборов.
var firstCart = new Cart(1, user.Id);
firstCart.AddItem(plate, 3);
firstCart.AddItem(cutlery, 2);
ProcessOrder("ЗАКАЗ №1", profile, firstCart);

// Заказ №2: повторная покупка для демонстрации неуспешной оплаты и повторной попытки.
var secondCart = new Cart(2, user.Id);
secondCart.AddItem(plate, 3);
secondCart.AddItem(cutlery, 2);
ProcessOrder("ЗАКАЗ №2 — ПЕРВАЯ ПОПЫТКА", profile, secondCart);

Console.WriteLine("\nПополнение денежного баланса на 2 300 УЕ...");
profile.UpdateBalance(profile.Balance with { Money = profile.Balance.Money + 2_300m });
Console.WriteLine($"Баланс после пополнения: {profile.Balance}");
ProcessOrder("ЗАКАЗ №2 — ПОВТОРНАЯ ПОПЫТКА", profile, secondCart);

Console.WriteLine("\n=== ИТОГ ===");
Console.WriteLine($"Денежный баланс: {profile.Balance.Money:N2} УЕ");
Console.WriteLine($"Баланс бонусов: {profile.Balance.Bonuses:N2}");
Console.WriteLine($"Количество купленных товарных позиций: {profile.Purchases.Count}");
foreach (CartItem item in profile.Purchases)
    Console.WriteLine($"  {item}");

var (deconstructedUser, purchasesCount) = profile;
Console.WriteLine($"Deconstruct профиля: {deconstructedUser.Login}, покупок-позиций: {purchasesCount}");

static void ProcessOrder(string title, UserProfile profile, Cart cart)
{
    Console.WriteLine($"\n--- {title} ---");
    Console.WriteLine($"Позиций в корзине (единиц товара): {cart.ItemCount}");
    foreach (CartItem item in cart.Items)
        Console.WriteLine($"  {item}");

    PricingResult price = cart.CalculatePrice(profile.PersonalDiscount);
    Console.WriteLine($"Исходная цена: {price.OriginalPrice:N2} УЕ");
    price.ShowDiscountAmount();
    price.ShowFinalPrice();

    BalanceState before = profile.Balance;
    bool paid = OrderService.TryPay(before, price, out BalanceState after,
        out decimal bonusPart, out decimal moneyPart, out string message);

    Console.WriteLine(message);
    if (!paid)
    {
        Console.WriteLine($"Списания не выполнены. Баланс сохранён: {profile.Balance}");
        Console.WriteLine($"Корзина сохранена, товарных единиц: {cart.ItemCount}");
        return;
    }

    Console.WriteLine($"Оплата бонусами: {bonusPart:N2}; деньгами: {moneyPart:N2} УЕ.");
    profile.UpdateBalance(after);
    profile.AddPurchases(cart.CreatePurchaseSnapshot());
    cart.Clear();
    Console.WriteLine($"Новый баланс: {profile.Balance}");
    Console.WriteLine($"Корзина очищена. Товарных единиц: {cart.ItemCount}; итоговая цена: {cart.CalculatePrice(profile.PersonalDiscount).FinalPrice:N2} УЕ.");

    // with создаёт новое значение структуры; старое значение before осталось доступным.
    BalanceState demonstratedWith = after with { BonusShare = after.BonusShare };
    Console.WriteLine($"Демонстрация with после оплаты: {demonstratedWith}");
}
