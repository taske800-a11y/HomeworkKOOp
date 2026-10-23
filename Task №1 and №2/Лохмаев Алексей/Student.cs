public class Student
{
    private string name;
    private int age;

    public Student(string name, int age)
    {
        this.name = name;
        this.age = age;
    }

    public string GetName()
    {
        return string.IsNullOrWhiteSpace(name) ? "Unknown" : name;
    }

    public int GetAge()
    {
        return age;
    }

    public void SetName(string name)
    {
        this.name = name;
    }

    public void SetAge(int age)
    {
        this.age = age;
    }
}
