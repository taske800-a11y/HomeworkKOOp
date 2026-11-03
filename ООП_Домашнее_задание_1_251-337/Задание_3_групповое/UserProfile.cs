using System.Collections.Generic;

public class UserProfile
{
    public User User { get; set; }
    public List<Contact> Contacts { get; set; }

    public UserProfile(User user, List<Contact> contacts)
    {
        User = user;
        Contacts = contacts;
    }

    // Деконструктор: (пользователь, количество контактов)
    public void Deconstruct(out User user, out int contactsCount)
    {
        user = User;
        contactsCount = Contacts.Count;
    }
}
