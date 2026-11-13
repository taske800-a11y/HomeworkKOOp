namespace IndividualAssignments;

public class StudentProgress
{
    private string _teacherName = string.Empty;
    private int _semesterPoints;

    public required string Subject { get; init; }

    public string TeacherName
    {
        get => _teacherName;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Имя преподавателя не может быть пустым.", nameof(value));

            _teacherName = value;
        }
    }

    public int SemesterPoints
    {
        get => _semesterPoints;
        set
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(
                    nameof(value),
                    "Баллы за семестр не могут быть отрицательными.");

            _semesterPoints = value;
        }
    }

    public bool NeedsRetake => SemesterPoints < 25;

    public string GetInfo() => GetInfo(includeIntProperty: true);

    public string GetInfo(bool includeIntProperty)
    {
        string info = $"Предмет: {Subject}; преподаватель: {TeacherName}";
        return includeIntProperty
            ? $"{info}; баллы: {SemesterPoints}"
            : info;
    }
}
