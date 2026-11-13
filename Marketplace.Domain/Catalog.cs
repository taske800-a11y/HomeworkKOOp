
using System;
using System.Collections.Generic;
using System.Linq;

namespace Marketplace.Domain;

public sealed class Catalog
{
    private readonly List<Product> _products = new();

    public void Add(Product product)
    {
        ArgumentNullException.ThrowIfNull(product);

        if (_products.Any(existing =>
                SameTitle(existing.GetTitle(), product.GetTitle())))
        {
            throw new ArgumentException(
                "Товар с таким названием уже есть в каталоге.",
                nameof(product));
        }

        _products.Add(product);
    }

    public bool Remove(string title)
    {
        string normalizedTitle = Normalize(title);

        int index = _products.FindIndex(product =>
            SameTitle(product.GetTitle(), normalizedTitle));

        if (index < 0)
            return false;

        _products.RemoveAt(index);
        return true;
    }

    public Product? Find(string title)
    {
        string normalizedTitle = Normalize(title);

        return _products.Find(product =>
            SameTitle(product.GetTitle(), normalizedTitle));
    }

    public IReadOnlyList<Product> GetProducts()
    {
        return _products.ToList().AsReadOnly();
    }

    public int GetCount()
    {
        return _products.Count;
    }

    private static string Normalize(string title)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(title);
        return title.Trim();
    }

    private static bool SameTitle(string left, string right)
    {
        return string.Equals(
            Normalize(left),
            Normalize(right),
            StringComparison.OrdinalIgnoreCase);
    }
}
