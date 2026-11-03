using System.Text;

namespace Individual.Lokhmaev;

internal static class Program
{
    private static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        Console.WriteLine("ИНДИВИДУАЛЬНАЯ РАБОТА");
        Console.WriteLine("Лохмаев Алексей Сергеевич | группа 251-337");

        DemonstrateAuditorium();
        DemonstrateReferenceAndValueTypes();
        ExplainMemoryQuestions();
    }

    private static void DemonstrateAuditorium()
    {
        Console.WriteLine("\n=== Задание 1. Структура Auditorium ===");

        // Основной конструктор: лаборатория с известным корпусом и числом мест.
        Auditorium laboratory = new("3D-204", "Корпус ИСИТ", 24);
        Show(laboratory);

        // default не вызывает пользовательский конструктор:
        // поля string получают null, а int — 0.
        Auditorium uninitialized = default;
        Console.WriteLine($"default: {uninitialized.Describe()}");

        // При присваивании структуры копируются её значения.
        Auditorium valueCopy = laboratory;
        valueCopy.Seats = 18;

        Console.WriteLine("После изменения обычной копии:");
        Show(laboratory);
        Show(valueCopy);

        // with создаёт отдельное значение на основе исходной структуры.
        Auditorium withCopy = laboratory with { Seats = 30 };
        Console.WriteLine($"Копия через with: {withCopy.Describe()}");
        Console.WriteLine($"Исходная аудитория после with: {laboratory.Describe()}");

        // Обычная передача структуры в метод не меняет исходную переменную.
        ReserveByValue(laboratory, 2);
        Console.WriteLine($"После ReserveByValue: {laboratory.Describe()}");

        // ref передаёт ссылку на исходную переменную структуры.
        Reserve(ref laboratory, 4);
        Console.WriteLine($"После Reserve(ref): {laboratory.Describe()}");

        Show(laboratory);
    }

    private static void ReserveByValue(Auditorium auditorium, int seats)
    {
        ValidateReservation(auditorium, seats);
        auditorium.Seats -= seats;
    }

    private static void Reserve(ref Auditorium auditorium, int seats)
    {
        ValidateReservation(auditorium, seats);
        auditorium.Seats -= seats;
    }

    private static void ValidateReservation(Auditorium auditorium, int seats)
    {
        if (seats <= 0)
            throw new ArgumentOutOfRangeException(nameof(seats), "Количество мест должно быть положительным.");

        if (seats > auditorium.Seats)
            throw new InvalidOperationException("Нельзя зарезервировать больше свободных мест, чем есть в аудитории.");
    }

    private static void Show(Auditorium auditorium)
    {
        Console.WriteLine($"Show -> {auditorium.Describe()}");
    }

    private static void DemonstrateReferenceAndValueTypes()
    {
        Console.WriteLine("\n=== Задание 2. Копирование ссылок и значений ===");

        // Класс: обе переменные ссылаются на один объект WorkshopStudent.
        WorkshopStudent firstReference = new("Алексей", "3D-моделирование");
        WorkshopStudent secondReference = firstReference;

        Console.WriteLine($"До изменения: first={firstReference}; second={secondReference}");
        secondReference.Specialization = "Разработка игр";
        Console.WriteLine($"После изменения через second: first={firstReference}; second={secondReference}");
        Console.WriteLine($"Один объект: {ReferenceEquals(firstReference, secondReference)}");

        // Структура: присваивание создаёт независимую копию полей.
        EnrollmentCard firstCard = new("ИСИТ-251337", 2);
        EnrollmentCard secondCard = firstCard;

        Console.WriteLine($"До изменения: first={firstCard}; second={secondCard}");
        secondCard.Semester = 3;
        Console.WriteLine($"После изменения копии: first={firstCard}; second={secondCard}");

        // Важный нюанс: поле структуры само является ссылкой на список.
        // При копировании структуры копируется ссылка, поэтому список общий.
        EquipmentLabel originalLabel = new(new List<string> { "ноутбук", "графический планшет" });
        EquipmentLabel copiedLabel = originalLabel;
        copiedLabel.Tags.Add("3D-принтер");

        Console.WriteLine($"Теги исходной структуры: {string.Join(", ", originalLabel.Tags)}");
        Console.WriteLine($"Теги копии структуры: {string.Join(", ", copiedLabel.Tags)}");
        Console.WriteLine("Сами структуры разные, но поле Tags указывает на один и тот же список.");
    }

    private static void ExplainMemoryQuestions()
    {
        Console.WriteLine("\n=== Краткие ответы ===");

        int localNumber = 42;
        Console.WriteLine($"Локальный int в примере: {localNumber}");
        Console.WriteLine("1. Объект класса обычно размещается в управляемой куче; локальный int хранит само значение, а физическое размещение зависит от контекста и оптимизаций JIT.");
        Console.WriteLine("2. GC может освободить объект, когда на него больше нельзя добраться из корней управляемой памяти (например, локальных ссылок, статических полей и активных стеков).");
        Console.WriteLine("3. При копировании структуры копируются значения всех полей; если поле — ссылка, копируется адрес ссылки, а не отдельная копия объекта.");
    }
}

public struct Auditorium
{
    public string? Number;
    public string? Building;
    public int Seats;

    public Auditorium(string number, string building, int seats)
    {
        if (string.IsNullOrWhiteSpace(number))
            throw new ArgumentException("Номер аудитории обязателен.", nameof(number));

        if (string.IsNullOrWhiteSpace(building))
            throw new ArgumentException("Название корпуса обязательно.", nameof(building));

        if (seats < 0)
            throw new ArgumentOutOfRangeException(nameof(seats), "Количество мест не может быть отрицательным.");

        Number = number;
        Building = building;
        Seats = seats;
    }

    public Auditorium(string number)
        : this(number, "Главный корпус", 0)
    {
    }

    public string Describe()
    {
        string number = Number ?? "(не задан)";
        string building = Building ?? "(не задан)";
        return $"ауд. {number}, {building}, мест: {Seats}";
    }
}

internal sealed class WorkshopStudent
{
    public string Name { get; }
    public string Specialization { get; set; }

    public WorkshopStudent(string name, string specialization)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Имя обязательно.", nameof(name));

        if (string.IsNullOrWhiteSpace(specialization))
            throw new ArgumentException("Направление обязательно.", nameof(specialization));

        Name = name;
        Specialization = specialization;
    }

    public override string ToString() => $"{Name}, направление: {Specialization}";
}

internal struct EnrollmentCard
{
    public string Code;
    public int Semester;

    public EnrollmentCard(string code, int semester)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Номер билета обязателен.", nameof(code));

        if (semester < 1)
            throw new ArgumentOutOfRangeException(nameof(semester));

        Code = code;
        Semester = semester;
    }

    public override string ToString() => $"{Code}, семестр: {Semester}";
}

internal struct EquipmentLabel
{
    public List<string> Tags;

    public EquipmentLabel(List<string> tags)
    {
        Tags = tags ?? throw new ArgumentNullException(nameof(tags));
    }
}
