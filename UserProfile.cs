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
}