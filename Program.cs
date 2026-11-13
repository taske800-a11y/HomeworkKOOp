using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Marketplace.Domain;
using Marketplace.DataAccess;
using Marketplace.Presentation;

namespace Marketplace;

internal class Program
{
    private static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.InputEncoding = System.Text.Encoding.UTF8;
        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("ru-RU");
        CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo("ru-RU");

        Console.WriteLine("==================================================");
        Console.WriteLine("       MARKETPLACE — ДЕМОНСТРАЦИЯ РАБОТЫ");
        Console.WriteLine("==================================================");

        // 1. Создание каталога и четырёх корректных товаров.
        PrintSection("1. СОЗДАНИЕ ТОВАРОВ");

        Catalog catalog = new();
        Product keyboard = new(1, "Механическая клавиатура", 4990);
        Product mouse = new(2, "Игровая мышь", 2490);
        Product headphones = new(3, "Наушники", 6990);
        Product monitorStand = new(4, "Подставка для монитора", 3190);

        catalog.Add(keyboard);
        catalog.Add(mouse);
        catalog.Add(headphones);
        catalog.Add(monitorStand);

        Console.WriteLine($"Успешно добавлено товаров: {catalog.GetCount()}");

        // 2. Валидация конструктора и SetPrice.
        PrintSection("2. ПРОВЕРКА ВАЛИДАЦИИ");

        RunValidationCheck("Создание товара с отрицательным ID",
            () => _ = new Product(-1, "Тестовый товар", 100));

        RunValidationCheck("Создание товара с пустым названием",
            () => _ = new Product(5, "   ", 100));

        RunValidationCheck("Создание товара с отрицательной ценой",
            () => _ = new Product(6, "Тестовый товар", -100));

        int originalPrice = mouse.GetPrice();
        RunValidationCheck("Установка отрицательной цены через SetPrice(-100)",
            () => mouse.SetPrice(-100));

        Console.WriteLine(
            $"Цена после отклонённого изменения: {mouse.GetPrice():N0} у.е. " +
            $"(до проверки: {originalPrice:N0} у.е.)");

        RunValidationCheck("Добавление товара с повторяющимся названием",
            () => catalog.Add(new Product(5, "  ИГРОВАЯ МЫШЬ  ", 1500)));

        RunValidationCheck("Добавление null в каталог",
            () => catalog.Add(null!));

        // 3. Поиск и удаление: успешные и отрицательные сценарии.
        PrintSection("3. ПОИСК И УДАЛЕНИЕ ТОВАРОВ");

        Product? foundProduct = catalog.Find("   ИГРОВАЯ МЫШЬ   ");
        Console.WriteLine(foundProduct is not null
            ? $"Найден товар: {foundProduct.GetInfo()}"
            : "Товар не найден.");

        Product? missingProduct = catalog.Find("Несуществующий товар");
        Console.WriteLine(missingProduct is null
            ? "Ожидаемый результат: Find вернул null для отсутствующего товара."
            : "ОШИБКА: найден товар, которого нет в каталоге.");

        bool removed = catalog.Remove("Механическая клавиатура");
        Console.WriteLine(removed
            ? "Товар «Механическая клавиатура» успешно удалён."
            : "Не удалось удалить товар.");

        bool removedMissing = catalog.Remove("Несуществующий товар");
        Console.WriteLine(
            $"Remove для отсутствующего товара вернул: {removedMissing} (ожидается false).");

        // Возвращаем товар, чтобы сохранить четыре позиции для дальнейшей демонстрации.
        catalog.Add(keyboard);
        Console.WriteLine($"Товаров в каталоге: {catalog.GetCount()}");

        // 4. Демонстрация связи старой корзины с новым каталогом.
        PrintSection("4. ИНТЕГРАЦИЯ КАТАЛОГА С КОРЗИНОЙ");

        Cart cart = new(1, 1);
        Product? cartProduct = catalog.Find("Наушники");

        if (cartProduct is not null)
        {
            cart.AddItem(cartProduct, 2);
            PricingResult cartPrice = cart.CalculatePrice(0.95m);

            Console.WriteLine(
                $"В корзину из каталога добавлено: {cartProduct.GetTitle()}, " +
                $"количество: {cart.ItemCount}.");

            Console.WriteLine(
                $"Стоимость двух товаров с множителем скидки 0,95: " +
                $"{cartPrice.FinalPrice.ToString("N2", CultureInfo.CurrentCulture)} у.е.");

            Console.WriteLine(
                $"Позиции снимка покупки: {cart.CreatePurchaseSnapshot().Count}");
        }

        // 5. Защита коллекции. Вызов Clear() намеренно не выполняется:
        // он не компилируется для IReadOnlyList<Product>.
        PrintSection("5. ЗАЩИТА ВНУТРЕННЕЙ КОЛЛЕКЦИИ");

        IReadOnlyList<Product> productsSnapshot = catalog.GetProducts();
        Console.WriteLine(
            $"Получено товаров через GetProducts(): {productsSnapshot.Count}");
        Console.WriteLine(
            "GetProducts() возвращает IReadOnlyList<Product> — " +
            "метода Clear() в этом контракте нет.");
        Console.WriteLine(
            $"Количество товаров в исходном каталоге: {catalog.GetCount()}");

        // 6. Сохранение и загрузка через интерфейс (DIP).
        PrintSection("6. СОХРАНЕНИЕ И ЗАГРУЗКА ДАННЫХ");

        string filePath = Path.Combine(
            AppContext.BaseDirectory, "Data", "products.csv");

        // Клиентский код зависит от IProductRepository, а не от реализации.
        IProductRepository repository = new FileProductRepository(filePath);
        repository.Save(catalog.GetProducts());

        Console.WriteLine("Каталог успешно сохранён.");
        Console.WriteLine($"Путь к файлу: {filePath}");

        List<Product> loadedProducts = repository.Load();
        Console.WriteLine($"Загружено товаров из файла: {loadedProducts.Count}");

        Catalog loadedCatalog = new();
        foreach (Product product in loadedProducts)
            loadedCatalog.Add(product);

        Console.WriteLine(
            $"Товаров в восстановленном каталоге: {loadedCatalog.GetCount()}");

        // 7. Печать и аналитика отделены от доменных сущностей (SRP).
        PrintSection("7. ВЫВОД ТОВАРОВ");

        CatalogPrinter printer = new();
        Console.WriteLine("Полный список товаров:");
        printer.PrintAll(loadedCatalog.GetProducts());

        Console.WriteLine();
        printer.PrintTop(loadedCatalog.GetProducts(), 2);

        PrintSection("8. АНАЛИТИЧЕСКИЙ ОТЧЁТ");

        CatalogReport report = new();
        double averagePrice = report.GetAveragePrice(loadedCatalog.GetProducts());

        Console.WriteLine(
            $"Средняя цена товаров: {averagePrice.ToString("N2", CultureInfo.CurrentCulture)} у.е.");
        Console.WriteLine();
        Console.WriteLine(report.GetSummary(loadedCatalog.GetProducts()));

        PrintSection("9. ПРОВЕРКА ПРОФИЛЯ И СКИДКИ");

        CustomerProfile customer = new()
        {
            FullName = "Анна Петрова",
            Email = "anna@example.com",
            Phone = "+7 (900) 123-45-67",
            Rating = 5
        };
        Console.WriteLine(customer.GetInfo(includeRating: true));
        Console.WriteLine(customer.GetContactInfo(includePhone: true));

        Discount discount = new()
        {
            Name = "Весенняя скидка",
            ValidUntil = DateTime.Now.AddDays(7),
            Percent = 10,
            MinSum = 5000
        };
        Console.WriteLine($"Цена заказа 6000 после скидки: {discount.Calculate(6000)} у.е.");
        RunValidationCheck("Установка Percent = 101 через свойство",
            () => discount.Percent = 101);
        RunValidationCheck("Установка MinSum = -1 через свойство",
            () => discount.MinSum = -1);

        PrintSection("ДЕМОНСТРАЦИЯ ЗАВЕРШЕНА");
        Console.WriteLine("Проверены:");
        Console.WriteLine(" - валидация Product и SetPrice;");
        Console.WriteLine(" - запрет null и дубликатов в Catalog;");
        Console.WriteLine(" - успешный и неуспешный поиск/удаление;");
        Console.WriteLine(" - интеграция Catalog и Cart;");
        Console.WriteLine(" - защита коллекции каталога;");
        Console.WriteLine(" - сохранение и загрузка через IProductRepository;");
        Console.WriteLine(" - раздельные вывод и расчёты.");
        Console.WriteLine(" - профиль клиента и проверка свойств Discount.");
        Console.WriteLine("==================================================");
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
            Console.WriteLine($"          Причина: {exception.Message}");
        }
    }

    private static void PrintSection(string title)
    {
        Console.WriteLine();
        Console.WriteLine("--------------------------------------------------");
        Console.WriteLine(title);
        Console.WriteLine("--------------------------------------------------");
    }
}
