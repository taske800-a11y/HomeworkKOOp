using System.Text;

Console.OutputEncoding = Encoding.UTF8;

Console.WriteLine("=== Индивидуальные задания Останина Платона Валерьевича ===");
Task1();
Task2();

static void Task1()
{
    Console.WriteLine("\n--- Задание 1: структура Auditorium ---");
    Auditorium auditorium = new("101", "Корпус Б", 60);
    Console.WriteLine(auditorium.Describe());

    Auditorium defaultAuditorium = default;
    // default обнуляет поля структуры: ссылки становятся null, числовые поля — 0.
    Console.WriteLine($"default: {defaultAuditorium.Describe()}");

    Auditorium copy = auditorium;
    copy.Seats = 100;
    Console.WriteLine($"Копирование значений: оригинал={auditorium.Seats}, копия={copy.Seats}");

    Auditorium withCopy = auditorium with { Seats = 80 };
    Console.WriteLine($"with: оригинал={auditorium.Seats}, копия={withCopy.Seats}");

    Show(auditorium);
    ReserveByValue(auditorium, 10);
    Console.WriteLine($"После передачи по значению: {auditorium.Seats}");
    Reserve(ref auditorium, 10);
    Console.WriteLine($"После передачи по ref: {auditorium.Seats}");
}

static void Reserve(ref Auditorium auditorium, int seats)
{
    if (seats <= 0 || seats > auditorium.Seats)
        throw new ArgumentOutOfRangeException(nameof(seats));
    auditorium.Seats -= seats;
}

static void ReserveByValue(Auditorium auditorium, int seats) => auditorium.Seats -= seats;
static void Show(Auditorium auditorium) => Console.WriteLine($"Show: {auditorium.Describe()}");

static void Task2()
{
    Console.WriteLine("\n--- Задание 2: класс и структура ---");
    var first = new Student("Иван", 19);
    var second = first;
    second.Age = 20;
    Console.WriteLine($"Класс: first={first}; second={second}; одна ссылка={ReferenceEquals(first, second)}");

    StudentCard original = new("Б-2417", 2);
    StudentCard copy = original;
    copy.Course = 3;
    Console.WriteLine($"Структура: оригинал={original}; копия={copy}");

    Console.WriteLine("1. Экземпляр класса размещается в управляемой куче; локальная переменная int хранит значение (место хранения зависит от контекста выполнения).");
    Console.WriteLine("2. GC может освободить объект, когда тот становится недостижимым из корней управляемой памяти.");
    Console.WriteLine("3. При копировании структуры копируется ссылка в её поле, а не объект, на который эта ссылка указывает.");
}

struct Auditorium
{
    public string? Number;
    public string? Building;
    public int Seats;

    public Auditorium(string number, string building, int seats)
    {
        if (string.IsNullOrWhiteSpace(number)) throw new ArgumentException("Номер обязателен.");
        if (string.IsNullOrWhiteSpace(building)) throw new ArgumentException("Корпус обязателен.");
        if (seats < 0) throw new ArgumentOutOfRangeException(nameof(seats));
        Number = number;
        Building = building;
        Seats = seats;
    }

    public Auditorium(string number) : this(number, "Главный корпус", 0) { }
    public string Describe() => $"ауд. {Number ?? "(null)"}, {Building ?? "(null)"}, мест: {Seats}";
}

sealed class Student
{
    public string Name { get; }
    public int Age { get; set; }
    public Student(string name, int age)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException(nameof(name));
        if (age < 0) throw new ArgumentOutOfRangeException(nameof(age));
        Name = name;
        Age = age;
    }
    public override string ToString() => $"{Name}, {Age} лет";
}

struct StudentCard
{
    public string Number;
    public int Course;
    public StudentCard(string number, int course)
    {
        Number = number;
        Course = course;
    }
    public override string ToString() => $"билет {Number}, {Course} курс";
}
