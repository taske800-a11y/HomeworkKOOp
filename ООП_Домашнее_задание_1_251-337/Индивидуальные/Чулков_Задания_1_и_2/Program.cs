using System;

// ===================== ЗАДАНИЕ 1: класс Book =====================
class Book
{
    // Свойства (init позволяет использовать инициализатор объекта)
    public string Title { get; init; }
    public string Author { get; init; }
    public int Year { get; init; }
    public bool IsRead { get; set; }

    // 1. Главный конструктор (title, author, year) - вся инициализация в нём
    public Book(string title, string author, int year)
    {
        Title = title;
        Author = author;
        Year = year;
        IsRead = false;
    }

    // 2. Упрощённый конструктор (title) - делегирует главному
    //    автор "неизвестен", год 0
    public Book(string title) : this(title, "неизвестен", 0) { }

    // Конструктор без параметров нужен для инициализатора объекта
    public Book() : this("Без названия") { }

    // 4. Деконструктор: распаковка в (title, isRead)
    public void Deconstruct(out string title, out bool isRead)
    {
        title = Title;
        isRead = IsRead;
    }

    public override string ToString() =>
        $"\"{Title}\", автор: {Author}, год: {Year}, прочитана: {(IsRead ? "да" : "нет")}";
}

// ===================== Студент =====================
class Student
{
    private string name;

    public Student(string name)
    {
        this.name = name;
    }

    public string GetName() => name;
}

// ===================== ЗАДАНИЕ 2: хранилище книг =====================
class StudentBookStorage
{
    // Владелец хранилища
    private Student? StorageOwner;

    // Книга в хранилище
    private Book? StoredBook;

    // Главный конструктор - вся инициализация в нём
    public StudentBookStorage(Student? owner, Book? book)
    {
        StorageOwner = owner;
        StoredBook = book;
    }

    // Упрощённые конструкторы делегируют главному
    public StudentBookStorage(Student? owner) : this(owner, null) { }
    public StudentBookStorage() : this((Student?)null) { }

    // Извлечение книги из хранилища
    public Book? GetABook(Student? student)
    {
        if (IsValidAccess(student))
        {
            return StoredBook;
        }
        return null;
    }

    // Размещение книги в хранилище
    // Неопределённую книгу (null) разместить нельзя.
    // Размещать может только владелец (если он определён) - логика та же, что и в GetABook.
    public bool PutABook(Student? student, Book? book)
    {
        // Неопределённая книга - размещение невозможно
        if (book == null)
        {
            return false;
        }

        // Проверяем доступ так же, как при извлечении
        if (IsValidAccess(student))
        {
            StoredBook = book;
            return true;
        }
        return false;
    }

    private bool IsValidAccess(Student? student)
    {
        // Нет студента - нет доступа!
        return student != null &&
               // Нет владельца - значит ничейное.
               (StorageOwner == null ||
               // Есть владелец и он перед нами - даём доступ.
               string.Equals(student.GetName(), StorageOwner.GetName()));
    }
}

// ===================== Демонстрация =====================
class Program
{
    static void Main()
    {
        Console.WriteLine("=== ЗАДАНИЕ 1: Book ===");

        // Главный конструктор
        Book b1 = new Book("Война и мир", "Л. Толстой", 1869);
        Console.WriteLine(b1);

        // Упрощённый конструктор (только title)
        Book b2 = new Book("Загадочная книга");
        Console.WriteLine(b2);

        // Инициализатор объекта
        Book b3 = new Book
        {
            Title = "Мастер и Маргарита",
            Author = "М. Булгаков",
            Year = 1967,
            IsRead = true
        };
        Console.WriteLine(b3);

        // Деконструкция книги
        var (title, isRead) = b3;
        Console.WriteLine($"Распаковано: title = {title}, isRead = {isRead}");

        Console.WriteLine();
        Console.WriteLine("=== ЗАДАНИЕ 2: StudentBookStorage ===");

        Student owner = new Student("Анна");
        Student stranger = new Student("Борис");

        // Хранилище с владельцем, пустое
        StudentBookStorage storage = new StudentBookStorage(owner);

        // 1) Владелец размещает книгу
        bool r1 = storage.PutABook(owner, b1);
        Console.WriteLine($"Владелец разместил книгу: {r1}");

        // 2) Чужой пытается разместить книгу
        bool r2 = storage.PutABook(stranger, b2);
        Console.WriteLine($"Чужой разместил книгу: {r2}");

        // 3) Попытка разместить null-книгу
        bool r3 = storage.PutABook(owner, null);
        Console.WriteLine($"Размещение неопределённой книги: {r3}");

        // 4) Студент не определён
        bool r4 = storage.PutABook(null, b2);
        Console.WriteLine($"Размещение без студента: {r4}");

        // 5) Извлечение владельцем
        Book? got1 = storage.GetABook(owner);
        Console.WriteLine($"Владелец извлёк: {(got1 != null ? got1.ToString() : "ничего")}");

        // 6) Извлечение чужим
        Book? got2 = storage.GetABook(stranger);
        Console.WriteLine($"Чужой извлёк: {(got2 != null ? got2.ToString() : "ничего")}");

        // 7) Ничейное хранилище: размещать и извлекать может любой студент
        StudentBookStorage publicStorage = new StudentBookStorage();
        bool r5 = publicStorage.PutABook(stranger, b3);
        Console.WriteLine($"Ничейное хранилище, размещение: {r5}");
        Book? got3 = publicStorage.GetABook(owner);
        Console.WriteLine($"Ничейное хранилище, извлечение: {(got3 != null ? got3.ToString() : "ничего")}");
    }
}