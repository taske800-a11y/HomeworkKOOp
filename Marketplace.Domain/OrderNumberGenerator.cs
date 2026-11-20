namespace Marketplace.Domain;


public static class OrderNumberGenerator
{
    private static int lastNumber;

    static OrderNumberGenerator()
    {
        lastNumber = 1000;
    }

    public static int LastNumber => lastNumber;

    public static int Next() => ++lastNumber;
}