public class CommunicationChannel
{
    public int Id { get; set; }
    public string Name { get; set; }

    public CommunicationChannel(int id, string name)
    {
        Id = id;
        Name = name;
    }
}
