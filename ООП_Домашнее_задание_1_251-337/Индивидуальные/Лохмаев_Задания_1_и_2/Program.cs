Console.WriteLine("==================================");
Console.WriteLine("          ЗАДАНИЕ 1: BOOK");
Console.WriteLine("==================================");

// Создание книги с основным конструктором и инициализатором объекта
var book1 = new Book("Чистый код", "Роберт Мартин", 2008)
{
    IsRead = true
};

// Создание книги с упрощённым конструктором
var book2 = new Book("Неизвестная книга");

Console.WriteLine($"Название: {book1.Title}");
Console.WriteLine($"Автор: {book1.Author}");
Console.WriteLine($"Год издания: {book1.Year}");
Console.WriteLine($"Прочитана: {book1.IsRead}");

// Демонстрация деконструктора
var (title, isRead) = book1;
Console.WriteLine($"Результат Deconstruct: {title}, прочитана: {isRead}");

Console.WriteLine($"\nКнига с упрощённым конструктором: {book2.Title}");
Console.WriteLine($"Автор: {book2.Author}, год: {book2.Year}");

Console.WriteLine("\n==================================");
Console.WriteLine("    ЗАДАНИЕ 2: BOOK STORAGE");
Console.WriteLine("==================================");

var owner = new Student("Иван Иванов", 20);
var anotherStudent = new Student("Пётр Петров", 21);
var storage = new StudentBookStorage(owner);

// Владелец успешно размещает книгу
bool putResult = storage.PutABook(owner, book1);
Console.WriteLine($"Владелец размещает книгу: {putResult}");

// Другой студент не может разместить книгу в чужом хранилище
bool deniedResult = storage.PutABook(anotherStudent, book2);
Console.WriteLine($"Другой студент размещает книгу: {deniedResult}");

// Владелец получает книгу
Book? receivedBook = storage.GetABook(owner);
if (receivedBook != null)
{
    Console.WriteLine($"Получена книга: {receivedBook.Title}");
}
else
{
    Console.WriteLine("Книга не получена.");
}

// Демонстрация запрета на размещение null
bool nullResult = storage.PutABook(owner, null);
Console.WriteLine($"Попытка разместить null: {nullResult}");

Console.WriteLine("\nДемонстрация заданий 1 и 2 завершена.");
