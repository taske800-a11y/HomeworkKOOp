using Marketplace.Domain;

namespace Marketplace.Services;

public static class CatalogPrinter
{
    public static void Print(IEnumerable<Product> products)
    {
        ArgumentNullException.ThrowIfNull(products);

        foreach (Product product in products)
        {
            ArgumentNullException.ThrowIfNull(product);
            Console.WriteLine($"{product.Id}. {product.Name} — {PriceFormatter.Format(product.UnitPrice)}");
        }
    }

    public static void PrintMostExpensive(IEnumerable<Product> products, int count = 3)
    {
        ArgumentNullException.ThrowIfNull(products);
        if (count <= 0)
            throw new ArgumentOutOfRangeException(nameof(count));

        Product[] catalog = products.Select(product =>
        {
            ArgumentNullException.ThrowIfNull(product);
            return product;
        }).ToArray();

        Console.WriteLine("Самые дорогие товары:");
        foreach (Product product in catalog
                     .OrderByDescending(product => product.UnitPrice)
                     .ThenBy(product => product.Id)
                     .Take(count))
        {
            Console.WriteLine($"{product.Id}. {product.Name} — {PriceFormatter.Format(product.UnitPrice)}");
        }
    }
}

file static class PriceFormatter
{
    public static string Format(decimal price) => $"{price:N2} УЕ";
}
