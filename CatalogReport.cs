using System.Globalization;
using Marketplace.Domain;

namespace Marketplace.Presentation;

public class CatalogReport
{
    public double GetAveragePrice(IEnumerable<Product> products)
    {
        ArgumentNullException.ThrowIfNull(products);

        List<Product> list = products.ToList();
        if (list.Count == 0)
        {
            return 0.0;
        }

        return list.Average(product => (double)product.GetPrice());
    }

    public string GetSummary(IEnumerable<Product> products)
    {
        ArgumentNullException.ThrowIfNull(products);

        List<Product> list = products.ToList();
        if (list.Count == 0)
        {
            return "Каталог пуст.";
        }

        Product cheapest = list.MinBy(product => product.GetPrice())!;
        Product mostExpensive = list.MaxBy(product => product.GetPrice())!;
        double average = GetAveragePrice(list);
        long total = list.Sum(product => (long)product.GetPrice());

        return $"Товаров: {list.Count}; " +
               $"средняя цена: {average.ToString("0.00", CultureInfo.InvariantCulture)} у.е.; " +
               $"самый дешёвый: {cheapest.GetTitle()} ({cheapest.GetPrice()} у.е.); " +
               $"самый дорогой: {mostExpensive.GetTitle()} ({mostExpensive.GetPrice()} у.е.); " +
               $"общая стоимость: {total} у.е.";
    }
}
