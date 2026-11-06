namespace Marketplace.Domain;

public class Catalog
{
    private readonly List<Product> products = new();

    public void Add(Product product)
    {
        ArgumentNullException.ThrowIfNull(product);

        if (products.Any(existing => SameTitle(existing.GetTitle(), product.GetTitle())))
        {
            throw new ArgumentException("Товар с таким названием уже есть в каталоге.", nameof(product));
        }

        products.Add(product);
    }

    public bool Remove(string title)
    {
        int index = products.FindIndex(product => SameTitle(product.GetTitle(), title));
        if (index < 0)
        {
            return false;
        }

        products.RemoveAt(index);
        return true;
    }

    public Product? Find(string title)
    {
        return products.Find(product => SameTitle(product.GetTitle(), title));
    }

    public IReadOnlyList<Product> GetProducts() => products.ToList().AsReadOnly();

    public int GetCount()
    {
        return products.Count;
    }

    private static string Normalize(string title)
    {
        ArgumentNullException.ThrowIfNull(title);
        return title.Trim();
    }

    private static bool SameTitle(string left, string right)
    {
        return string.Equals(Normalize(left), Normalize(right), StringComparison.OrdinalIgnoreCase);
    }
}
