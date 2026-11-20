using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Marketplace.Domain;

namespace Marketplace.Presentation;

public sealed class CatalogReport
{
    private static readonly CultureInfo PriceCulture =
        CultureInfo.GetCultureInfo("ru-RU");

    public double GetAveragePrice(IEnumerable<Product> products)
    {
        List<Product> list = ValidateProducts(products);

        if (list.Count == 0)
            return 0.0;

        return list.Average(product => product.Price);
    }

    public string GetSummary(IEnumerable<Product> products)
    {
        List<Product> list = ValidateProducts(products);

        if (list.Count == 0)
            return "Каталог пуст.";

        Product cheapest = list.MinBy(product => product.Price)!;
        Product mostExpensive = list.MaxBy(product => product.Price)!;
        double average = GetAveragePrice(list);
        long total = list.Sum(product => (long)product.Price);

        return $"Товаров: {list.Count}; " +
               $"средняя цена: {average.ToString("N2", PriceCulture)} у.е.; " +
               $"самый дешёвый: {cheapest.Title} ({FormatPrice(cheapest.Price)}); " +
               $"самый дорогой: {mostExpensive.Title} ({FormatPrice(mostExpensive.Price)}); " +
               $"общая стоимость: {FormatPrice(total)}.";
    }

    private static string FormatPrice(long price) =>
        price.ToString("N0", PriceCulture) + " у.е.";

    private static List<Product> ValidateProducts(IEnumerable<Product> products)
    {
        ArgumentNullException.ThrowIfNull(products);

        List<Product> list = products.ToList();

        foreach (Product product in list)
            ArgumentNullException.ThrowIfNull(product);

        return list;
    }
}
