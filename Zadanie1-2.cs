// книга
class Book
{
    public string Title { get; set; }
    public string Author { get; set; }
    public int Year { get; set; }
    public bool IsRead { get; set; }

    // главный конструктор 
    public Book(string title, string author, int year)
    {
        Title = title;
        Author = author;
        Year = year;
        IsRead = false;
    }

    // упрощённый конструктор 
    public Book(string title) : this(title, "неизвестен", 0) { }

    // Деконструктор (не путать с деструктором!): (Название, Прочитана ли)
    public void Deconstruct(out string title, out bool isRead)
    {
        title = Title;
        isRead = IsRead;
    }
}

// студент
class Student
{
    private const string UNKNOWN = "Unknown";

    // поля 
    private string? Name;
    private int Age;

    // конструктор без параметров
    public Student()
    {
        Name = UNKNOWN;
        Age = 0;
    }

    // конструктор с параметрами
    public Student(string name, int age)
    {
        Name = name;
        Age = age;
    }

    public string? GetName()
    {
        return string.IsNullOrEmpty(Name) ? UNKNOWN : Name;
    }

    public int GetAge()
    {
        return Age;
    }

    public void SetName(string Name)
    {
        this.Name = Name;
    }

    public void SetAge(int Age)
    {
        this.Age = Age;
    }
}

// хранилище книг 
class StudentBookStorage
{
    // владелец хранилища
    private Student? StorageOwner;
    // книга в хранилище
    private Book? StoredBook;

    public StudentBookStorage(Student? owner, Book? book)
    {
        StorageOwner = owner;
        StoredBook = book;
    }

    public StudentBookStorage(Student? owner) : this(owner, null) { }
    public StudentBookStorage() : this(null) { }

    // извлечение книги их хранилища
    public Book? GetABook(Student? student)
    {
        if (IsValidAccess(student))
        {
            return this.StoredBook;
        }
        return null;
    }

    // размещение гниги в хранилище
    // true, если книга размещена
    public bool PutABook(Student? student, Book? book)
    {
        // неопределённую книгу разместить нельзя
        if (book == null)
        {
            return false;
        }
        // проверка доступа
        if (IsValidAccess(student))
        {
            this.StoredBook = book;
            return true;
        }
        return false;
    }

    private bool IsValidAccess(Student? student)
    {
        // нет студента - нет доступа
        return student != null &&
            (StorageOwner == null ||
            // Есть владелец и он перед нами - даём доступ.
            string.Equals(student.GetName(), StorageOwner.GetName()));
    }
}

class Program
{
    static void Main()
    {
        Task1();
        Console.WriteLine();
        Task2();
    }

    // Задание 1: Book
    private static void Task1()
    {
        Console.WriteLine("=== Задание 1 ===");

        // главный конструктор
        Book full = new Book("Война и мир", "Л. Толстой", 1869);
        Show(full);

        // упрощённый конструктор
        Book simple = new Book("Без автора");
        Show(simple);

        // инициализатор объекта
        Book initialized = new Book("Мастер и Маргарита", "М. Булгаков", 1967)
        {
            IsRead = true
        };
        Show(initialized);

        // распаковка книги через Deconstruct
        (string title, bool isRead) = initialized;
        Console.WriteLine($"Распаковано: \"{title}\", прочитана: {isRead}");
    }

    // Задание 2: StudentBookStorage
    private static void Task2()
    {
        Console.WriteLine("=== Задание 2 ===");

        Student anna = new("Анна", 20);
        Student ivan = new("Иван", 22);
        Book book = new("Хоббит", "Дж. Толкин", 1937);
        Book another = new("Дюна", "Ф. Герберт", 1965);

        // хранилище с владельцем Анной
        StudentBookStorage storage = new(anna);

        Console.WriteLine($"Анна кладёт книгу: {storage.PutABook(anna, book)}");              // True
        Console.WriteLine($"Иван кладёт книгу: {storage.PutABook(ivan, another)}");           // False
        Console.WriteLine($"Анна кладёт null: {storage.PutABook(anna, null)}");               // False
        Console.WriteLine($"Никто (null) кладёт книгу: {storage.PutABook(null, another)}");   // False

        Console.WriteLine($"Иван достаёт книгу: {Describe(storage.GetABook(ivan))}");         // нет книги
        Console.WriteLine($"Анна достаёт книгу: {Describe(storage.GetABook(anna))}");         // Хоббит

        StudentBookStorage shared = new();
        Console.WriteLine($"Иван кладёт в ничейное: {shared.PutABook(ivan, another)}");       // True
        Console.WriteLine($"Анна достаёт из ничейного: {Describe(shared.GetABook(anna))}");   // Дюна
    }

    private static void Show(Book book)
    {
        Console.WriteLine($"Книга \"{book.Title}\", автор: {book.Author}, год: {book.Year}, прочитана: {book.IsRead}");
    }

    private static string Describe(Book? book)
    {
        return book == null ? "книги нет" : $"\"{book.Title}\"";
    }
}
