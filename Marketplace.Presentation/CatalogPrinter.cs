using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Marketplace.Domain;

namespace Marketplace.Presentation;

public class CatalogPrinter
{
    public void PrintAll(IEnumerable<Product> products)
    {
        ArgumentNullException.ThrowIfNull(products);

        foreach (Product product in products)
        {
            ArgumentNullException.ThrowIfNull(product);
            Console.WriteLine(PrepareLine(product));
        }
    }

    public void PrintTop(IEnumerable<Product> products, int count)
    {
        ArgumentNullException.ThrowIfNull(products);

        if (count <= 0)
            throw new ArgumentOutOfRangeException(nameof(count));

        Product[] catalog = products.ToArray();

        foreach (Product product in catalog)
            ArgumentNullException.ThrowIfNull(product);

        Console.WriteLine("Самые дорогие товары:");

        foreach (Product product in catalog
                     .OrderByDescending(product => product.GetPrice())
                     .ThenBy(product => product.GetId())
                     .Take(count))
        {
            Console.WriteLine(PrepareLine(product));
        }
    }

    private string PrepareLine(Product product)
    {
        return $"{product.GetId()}. {product.GetTitle()} — " +
               PriceFormatter.Format(product.GetPrice());
    }
}

// Модификатор file ограничивает вспомогательный форматтер этим файлом.
file static class PriceFormatter
{
    public static string Format(int price) =>
        price.ToString("N0", CultureInfo.GetCultureInfo("ru-RU")) + " у.е.";
}
