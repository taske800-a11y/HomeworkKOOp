struct Auditorium
{
    public string Number;
    public string Building;
    public int Seats;

    public Auditorium(string number, string building, int seats)
    {
        Number = number;
        Building = building;
        Seats = seats;
    }

    public Auditorium(string number) : this(number, "Главный корпус", 0) { }

    public string Describe() => $"ауд. {Number}, {Building}, мест: {Seats}";
}

class Program
{
    static void Main()
    {
        Console.WriteLine("--- 1. Конструктор с параметрами ---");
        Auditorium lecture = new Auditorium("101", "Корпус А", 120);
        Console.WriteLine(lecture.Describe());

        Auditorium simple = new Auditorium("205");
        Console.WriteLine(simple.Describe());

        Console.WriteLine("\n--- 2. default ---");
        Auditorium empty = default;
        Console.WriteLine(empty.Describe());

        Console.WriteLine("\n--- 3. Копия присваиванием ---");
        Auditorium copy = lecture;
        copy.Seats = 60;
        Console.WriteLine($"Оригинал: {lecture.Describe()}");
        Console.WriteLine($"Копия:    {copy.Describe()}");

        Console.WriteLine("\n--- 4. Копия через with ---");
        Auditorium withCopy = lecture with { Seats = 60 };
        Console.WriteLine($"Оригинал: {lecture.Describe()}");
        Console.WriteLine($"Копия:    {withCopy.Describe()}");

        Console.WriteLine("\n--- 5. ref против передачи по значению ---");
        Show(lecture);

        ReserveByValue(lecture, 20);
        Console.Write("После ReserveByValue: ");
        Show(lecture);

        Reserve(ref lecture, 20);
        Console.Write("После Reserve(ref): ");
        Show(lecture);
    }

    static void Reserve(ref Auditorium auditorium, int seats)
    {
        if (seats <= 0 || seats > auditorium.Seats)
        {
            Console.WriteLine($"Нельзя забронировать {seats} мест: свободно {auditorium.Seats}");
            return;
        }
        auditorium.Seats -= seats;
    }

    static void ReserveByValue(Auditorium auditorium, int seats)
    {
        auditorium.Seats -= seats;
    }

    static void Show(Auditorium auditorium)
    {
        Console.WriteLine(auditorium.Describe());
    }
}
