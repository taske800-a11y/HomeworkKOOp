public class StudentBookStorage
{
    private Student? StorageOwner;
    private Book? StoredBook;

    public StudentBookStorage(Student? owner, Book? book)
    {
        StorageOwner = owner;
        StoredBook = book;
    }

    public StudentBookStorage(Student? owner) : this(owner, null)
    {
    }

    public StudentBookStorage() : this(null, null)
    {
    }

    // Размещать можно только существующую книгу и при наличии доступа
    public bool PutABook(Student? student, Book? book)
    {
        if (book == null || !IsValidAccess(student))
        {
            return false;
        }

        StoredBook = book;
        return true;
    }

    // Получить книгу может только пользователь с доступом
    public Book? GetABook(Student? student)
    {
        if (IsValidAccess(student))
        {
            return StoredBook;
        }

        return null;
    }

    private bool IsValidAccess(Student? student)
    {
        return student != null &&
               (StorageOwner == null ||
                string.Equals(student.GetName(), StorageOwner.GetName(),
                    StringComparison.Ordinal));
    }
}
