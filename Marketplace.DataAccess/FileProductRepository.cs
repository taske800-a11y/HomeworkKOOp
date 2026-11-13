
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Marketplace.Domain;

namespace Marketplace.DataAccess;

// Формат строки: ID;Цена;Название в Base64.
internal sealed class FileProductRepository : IProductRepository
{
    private readonly string filePath;

    public FileProductRepository(string filePath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath);

        this.filePath = Path.GetFullPath(filePath);
    }

    public void Save(IEnumerable<Product> products)
    {
        ArgumentNullException.ThrowIfNull(products);

        string[] lines = products
            .Select(Serialize)
            .ToArray();

        string? directory = Path.GetDirectoryName(filePath);

        if (!string.IsNullOrEmpty(directory))
            Directory.CreateDirectory(directory);

        File.WriteAllLines(filePath, lines, Encoding.UTF8);
    }

    public List<Product> Load()
    {
        if (!File.Exists(filePath))
            return new List<Product>();

        List<Product> products = new();

        int lineNumber = 0;

        foreach (string line in File.ReadLines(filePath))
        {
            lineNumber++;

            if (string.IsNullOrWhiteSpace(line))
                continue;

            try
            {
                products.Add(Deserialize(line));
            }
            catch (Exception ex) when (
                ex is FormatException ||
                ex is ArgumentException ||
                ex is DecoderFallbackException)
            {
                throw new FormatException(
                    $"Ошибка чтения товара в строке {lineNumber}.", ex);
            }
        }

        return products;
    }

    private static string Serialize(Product product)
    {
        ArgumentNullException.ThrowIfNull(product);

        string encodedTitle = Convert.ToBase64String(
            Encoding.UTF8.GetBytes(product.GetTitle()));

        return $"{product.GetId()};{product.GetPrice()};{encodedTitle}";
    }

    private static Product Deserialize(string line)
    {
        string[] parts = line.Split(';');

        if (parts.Length != 3)
            throw new FormatException("Неверный формат записи товара.");

        if (!int.TryParse(
                parts[0],
                NumberStyles.None,
                CultureInfo.InvariantCulture,
                out int id))
        {
            throw new FormatException("Некорректный идентификатор товара.");
        }

        if (!int.TryParse(
                parts[1],
                NumberStyles.None,
                CultureInfo.InvariantCulture,
                out int price))
        {
            throw new FormatException("Некорректная цена товара.");
        }

        string title = new UTF8Encoding(false, true).GetString(
            Convert.FromBase64String(parts[2]));

        return new Product(id, title, price);
    }
}
