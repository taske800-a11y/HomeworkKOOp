using System.Globalization;
namespace Marketplace.Services;
public static class PriceFormatter
{
    public static string Format(int value) => value.ToString("N0", CultureInfo.GetCultureInfo("ru-RU")) + " у.е.";
}
