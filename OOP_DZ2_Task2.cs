class Student
{
    public string Name;
    public int Age;

    public Student(string name, int age)
    {
        Name = name;
        Age = age;
    }

    public override string ToString() => $"Студент {Name}, {Age} лет";
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

class Program
{
    static void Main()
    {
        ClassReferenceDemo();
        StructCopyDemo();
        Answers();
    }

    static void ClassReferenceDemo()
    {
        Console.WriteLine("--- Пример 1: класс ---");

        Student first = new Student("Иван", 19);
        Student second = first;

        Console.WriteLine($"До:     first = {first}; second = {second}");

        second.Age = 20;

        Console.WriteLine($"После:  first = {first}; second = {second}");
    }

    static void StructCopyDemo()
    {
        Console.WriteLine("\n--- Пример 2: структура ---");

        StudentCard original = new StudentCard("Б-2417", 2);
        StudentCard copy = original;

        Console.WriteLine($"До:     original = {original}; copy = {copy}");

        copy.Course = 3;

        Console.WriteLine($"После:  original = {original}; copy = {copy}");
    }

    static void Answers()
    {
        Console.WriteLine("\n--- Ответы на вопросы ---");
        Console.WriteLine("1. Объект класса - в куче, локальный int - в стеке.");
        Console.WriteLine("2. Когда на объект не осталось достижимых ссылок.");
        Console.WriteLine("3. Копируется ссылка из поля, а не объект за ней: объект остаётся общим.");
    }
}
