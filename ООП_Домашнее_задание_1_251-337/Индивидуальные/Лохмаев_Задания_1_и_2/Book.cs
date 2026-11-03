public class Book
{
    public string Title { get; set; }
    public string Author { get; set; }
    public int Year { get; set; }
    public bool IsRead { get; set; }

    // Основной конструктор
    public Book(string title, string author, int year)
    {
        Title = title;
        Author = author;
        Year = year;
        IsRead = false;
    }

    // Упрощённый конструктор
    public Book(string title) : this(title, "неизвестен", 0)
    {
    }

    // Позволяет получить название книги и статус прочтения
    public void Deconstruct(out string title, out bool isRead)
    {
        title = Title;
        isRead = IsRead;
    }
}
