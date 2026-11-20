using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Marketplace.Domain;
using Marketplace.Services;

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
                     .OrderByDescending(product => product.Price)
                     .ThenBy(product => product.Id)
                     .Take(count))
        {
            Console.WriteLine(PrepareLine(product));
        }
    }

    private string PrepareLine(Product product)
    {
        return $"{product.Id}. {product.Title} — " +
               PriceFormatter.Format(product.Price);
    }
}

