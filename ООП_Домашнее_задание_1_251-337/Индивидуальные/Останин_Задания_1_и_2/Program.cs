using System;
using System.Text;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;   
        // ===== Задание 1: Book =====
        Console.WriteLine("=== Задание 1: Book ===");

        var simple = new Book("Без автора");                                              // упрощённый конструктор
        var full = new Book("Война и мир", "Толстой", 1869);                              // главный конструктор
        var init = new Book("Мастер и Маргарита", "Булгаков", 1967) { IsRead = true };    // инициализатор объекта

        Console.WriteLine($"simple: {simple.Title}, {simple.Author}, {simple.Year}");
        Console.WriteLine($"full: {full.Title}, {full.Author}, {full.Year}");
        Console.WriteLine($"init: {init.Title}, {init.Author}, {init.Year}, прочитана: {init.IsRead}");

        var (title, isRead) = init;                                                       // деконструкция
        Console.WriteLine($"Деконструкция: {title}, прочитана: {isRead}");

        // ===== Задание 2: StudentBookStorage =====
        Console.WriteLine();
        Console.WriteLine("=== Задание 2: хранилище с владельцем ===");

        var alice = new Student("Алиса");
        var bob = new Student("Боб");

        var storage = new StudentBookStorage(alice, full);

        Console.WriteLine($"1. Владелец извлекает:       {storage.GetABook(alice)?.Title ?? "null"}");
        Console.WriteLine($"2. Чужой извлекает:          {storage.GetABook(bob)?.Title ?? "null"}");
        Console.WriteLine($"3. Чужой размещает:          {storage.PutABook(bob, init)}");
        Console.WriteLine($"4. Владелец размещает null:  {storage.PutABook(alice, null!)}");
        Console.WriteLine($"5. null-студент размещает:   {storage.PutABook(null!, init)}");
        Console.WriteLine($"6. Владелец размещает книгу: {storage.PutABook(alice, init)}");
        Console.WriteLine($"7. Владелец извлекает:       {storage.GetABook(alice)?.Title ?? "null"}");
        Console.WriteLine($"8. Чужой извлекает снова:    {storage.GetABook(bob)?.Title ?? "null"}");

        Console.WriteLine();
        Console.WriteLine("=== Задание 2: хранилище без владельца ===");

        var openStorage = new StudentBookStorage();

        Console.WriteLine($"9.  Извлечение из пустого:   {openStorage.GetABook(bob)?.Title ?? "null"}");
        Console.WriteLine($"10. Боб размещает:           {openStorage.PutABook(bob, full)}");
        Console.WriteLine($"11. Алиса извлекает:         {openStorage.GetABook(alice)?.Title ?? "null"}");
    }
}

// Студент
class Student
{
    private string Name;

    public Student(string name)
    {
        Name = name;
    }

    public string GetName()
    {
        return Name;
    }
}

// Книга
class Book
{
    public string Title { get; set; }
    public string Author { get; set; }
    public int Year { get; set; }
    public bool IsRead { get; set; }

    public Book(string title, string author, int year)
    {
        Title = title;
        Author = author;
        Year = year;
    }

    public Book(string title) : this(title, "неизвестен", 0) { }

    public void Deconstruct(out string title, out bool isRead)
    {
        title = Title;
        isRead = IsRead;
    }
}

// Хранилище книг студента
class StudentBookStorage
{
    private Student? StorageOwner;
    private Book? StoredBook;

    public StudentBookStorage(Student? owner, Book? book)
    {
        StorageOwner = owner;
        StoredBook = book;
    }

    public StudentBookStorage(Student? owner) : this(owner, null) { }

    public StudentBookStorage() : this(null, null) { }

    public Book? GetABook(Student student)
    {
        if (IsValidAccess(student))
        {
            return StoredBook;
        }

        return null;
    }

    public bool PutABook(Student student, Book book)
    {
        if (book == null)
        {
            return false;
        }

        if (!IsValidAccess(student))
        {
            return false;
        }

        StoredBook = book;
        return true;
    }

    private bool IsValidAccess(Student student)
    {
        return student != null &&
               (StorageOwner == null ||
                string.Equals(student.GetName(), StorageOwner.GetName()));
    }
}