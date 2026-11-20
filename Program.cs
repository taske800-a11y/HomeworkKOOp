using System;
using System.Collections.Generic;
using System.Globalization;
using Marketplace.Domain;
using Marketplace.Services;

namespace Marketplace;

internal class Program
{
    private static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("ru-RU");

        PrintSection("1. СОЗДАНИЕ ТОВАРОВ");

        Product keyboard = new()
        {
            Id = 1,
            Title = "Механическая клавиатура",
            Article = "KEY-001",
            Price = 4990
        };

        Product mouse = new()
        {
            Id = 2,
            Title = "Игровая мышь",
            Article = "MOU-002",
            Price = 1490
        };

        Product cable = new()
        {
            Id = 3,
            Title = "USB-кабель",
            Article = "CAB-003",
            Price = 19
        };

        List<Product> products = new() { keyboard, mouse, cable };

        foreach (Product product in products)
        {
            Console.WriteLine(product.GetInfo());
            Console.WriteLine($"Бюджетный товар: {(product.IsBudget ? "да" : "нет")}");
        }

        PrintSection("2. СОЗДАНИЕ ПОКУПАТЕЛЕЙ");

        CustomerProfile firstCustomer = new()
        {
            FullName = "Иван Петров",
            Phone = "+7 (900) 123-45-67",
            Email = "ivan@example.com",
            Rating = 5
        };

        CustomerProfile secondCustomer = new()
        {
            FullName = "Мария Смирнова",
            Phone = "+7 (901) 765-43-21",
            Email = "maria@example.com",
            Rating = 3
        };

        Console.WriteLine(firstCustomer.GetInfo());
        Console.WriteLine(firstCustomer.GetInfo(includeRating: true));
        Console.WriteLine(firstCustomer.GetContactInfo(includePhone: true));
        Console.WriteLine($"Проверенный покупатель: {(firstCustomer.IsTrusted ? "да" : "нет")}");
        Console.WriteLine(secondCustomer.GetInfo(includeRating: true));

        PrintSection("3. ПЕРЕГРУЗКИ GETINFO");

        Console.WriteLine("Товар без цены:");
        Console.WriteLine(keyboard.GetInfo(includeIntProperty: false));
        Console.WriteLine("Товар с ценой:");
        Console.WriteLine(keyboard.GetInfo());

        PrintSection("4. РАБОТА СО СКИДКОЙ");

        Discount discount = new()
        {
            Name = "Сезонная скидка",
            Percent = 15m,
            MinSum = 1000,
            ValidUntil = DateTime.Now.AddDays(7)
        };

        int orderTotal = keyboard.Price + mouse.Price;
        Console.WriteLine($"Скидка активна: {(discount.IsActive ? "да" : "нет")}");
        Console.WriteLine($"Исходная сумма заказа: {orderTotal:N0} у.е.");
        Console.WriteLine($"Сумма со стандартной скидкой: {discount.Calculate(orderTotal):N0} у.е.");
        Console.WriteLine($"Сумма со скидкой 20%: {discount.Calculate(orderTotal, 20m):N0} у.е.");

        PrintSection("5. ФОРМИРОВАНИЕ КОРЗИНЫ И ОТЧЁТЫ");

        List<CartItem> cartItems = new()
        {
            new CartItem(productId: keyboard.Id, quantity: 1, pricePerUnit: keyboard.Price),
            new CartItem(productId: mouse.Id, quantity: 2, pricePerUnit: mouse.Price),
            new CartItem(productId: cable.Id, quantity: 3, pricePerUnit: cable.Price)
        };

        CheckoutService checkoutService = new();

        Console.WriteLine($"Общая стоимость корзины: {checkoutService.GetTotal(cartItems):N0} у.е.");
        Console.WriteLine();
        Console.WriteLine("Краткий отчёт:");
        Console.WriteLine(checkoutService.GetReport(cartItems));
        Console.WriteLine();
        Console.WriteLine("Подробный отчёт:");
        Console.WriteLine(checkoutService.GetReport(cartItems, includeLines: true));

        IReadOnlyList<CartItem> cheaperItems = checkoutService.FindCheaperThan(cartItems, 100);
        Console.WriteLine();
        Console.WriteLine($"Позиции дешевле 100 у.е. по цене за единицу: {cheaperItems.Count}");

        PrintSection("6. ПРОВЕРКА ВАЛИДАЦИИ");

        RunValidationCheck(
            "Попытка установить отрицательную цену",
            () => keyboard.Price = -10);

        RunValidationCheck(
            "Попытка установить пустой артикул",
            () => keyboard.Article = " ");

        RunValidationCheck(
            "Попытка установить рейтинг вне диапазона 0..5",
            () => firstCustomer.Rating = 6);

        RunValidationCheck(
            "Попытка установить недопустимый статус",
            () => keyboard.ChangeStatus("Продано"));

        PrintSection("7. ЗАЩИТА СВОЙСТВ");

        Console.WriteLine($"Title до попытки изменения: {keyboard.Title}");
        Console.WriteLine($"Status до изменения: {keyboard.Status}");

        keyboard.ChangeStatus("Под заказ");
        Console.WriteLine($"Status после ChangeStatus: {keyboard.Status}");

        // Иллюстрация попытки вызвать сеттер для неизменяемого свойства.
        // keyboard.Title = "Новое название";
        // Редактор сообщает об ошибке компиляции: свойство Title доступно только для чтения (init).

        // Иллюстрация попытки вызвать сеттер для закрытого свойства.
        // keyboard.Status = "Продан";
        // Редактор сообщает об ошибке компиляции: set свойства Status недоступен вне Product.

        Console.WriteLine("Title задаётся только при создании объекта.");
        Console.WriteLine("Status изменяется только через ChangeStatus().");

        PrintSection("ДЕМОНСТРАЦИЯ ЗАВЕРШЕНА");
    }

    private static void RunValidationCheck(string description, Action action)
    {
        try
        {
            action();
            Console.WriteLine($"[НЕ ПРОЙДЕНО] {description}");
        }
        catch (ArgumentException exception)
        {
            Console.WriteLine($"[УСПЕХ] {description}");
            Console.WriteLine($"Причина: {exception.Message}");
        }
    }

    private static void PrintSection(string title)
    {
        Console.WriteLine();
        Console.WriteLine("==================================================");
        Console.WriteLine(title);
        Console.WriteLine("==================================================");
    }
}
