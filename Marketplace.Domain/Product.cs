using System;
using System.Collections.Generic;

namespace Marketplace.Domain;

public sealed class Product
{
    private static readonly HashSet<string> AllowedStatuses = new(StringComparer.Ordinal)
    { "В наличии", "Нет в наличии", "Под заказ" };
    private string _title = string.Empty;
    private string _article = string.Empty;
    private int _price;

    public required int Id { get; init; }
    public required string Title
    {
        get => _title;
        init
        {
            if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Название товара обязательно.", nameof(value));
            _title = value.Trim();
        }
    }
    public required string Article
    {
        get => _article;
        set
        {
            if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Артикул обязателен.", nameof(value));
            _article = value.Trim();
        }
    }
    public int Price
    {
        get => _price;
        set { if (value < 0) throw new ArgumentOutOfRangeException(nameof(value), "Цена не может быть отрицательной."); _price = value; }
    }
    public bool IsBudget => Price < 25;
    public string Status { get; private set; } = "В наличии";
    public Product() { }
    public int GetId() => Id;
    public string GetTitle() => Title;
    public int GetPrice() => Price;
    public void SetPrice(int price) => Price = price;
    public string GetInfo() => GetInfo(true);
    public string GetInfo(bool includeIntProperty)
    {
        string info = $"Товар: {Title}, артикул: {Article}, статус: {Status}";
        return includeIntProperty ? $"{info}, цена: {Price}" : info;
    }
    public void ChangeStatus(string newStatus) { EnsureStatus(newStatus); Status = newStatus.Trim(); }
    private static void EnsureStatus(string value)
    { if (string.IsNullOrWhiteSpace(value) || !AllowedStatuses.Contains(value.Trim())) throw new ArgumentException("Недопустимый статус товара.", nameof(value)); }
}
