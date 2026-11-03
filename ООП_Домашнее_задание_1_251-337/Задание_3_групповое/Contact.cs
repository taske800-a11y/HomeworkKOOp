public class Contact
{
    //ID
    public int Id { get; set; }

    //ID канала
    public int CommunicationChannelId { get; set; }

    // Идентификатор в канале
    public string Value { get; set; }

    public Contact(int id, int communicationChannelId, string value)
    {
        Id = id;
        CommunicationChannelId = communicationChannelId;
        Value = value;
    }
}