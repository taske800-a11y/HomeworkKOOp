using System;
using System.Collections.Generic;
using System.Linq;

var channels = new List<CommunicationChannel>
{
    new CommunicationChannel(1, "Email"),
    new CommunicationChannel(2, "Telegram"),
    new CommunicationChannel(3, "Телефон")
};

var user = new User(
    1,
    "Иванов Иван Иванович",
    "ivanov",
    "password123"
);

var contacts = new List<Contact>
{
    new Contact(1, 1, "ivanov@example.com"),
    new Contact(2, 2, "@ivanov"),
    new Contact(3, 3, "+7 900 123-45-67")
};

var profile = new UserProfile(user, contacts);

Console.WriteLine("==================================");
Console.WriteLine("       ПРОФИЛЬ ПОЛЬЗОВАТЕЛЯ");
Console.WriteLine("==================================");

Console.WriteLine($"ID: {profile.User.Id}");
Console.WriteLine($"ФИО: {profile.User.FullName}");
Console.WriteLine($"Логин: {profile.User.Login}");

Console.WriteLine("\nКаналы связи:");

foreach (var channel in channels)
{
    Console.WriteLine($"ID: {channel.Id} | Название: {channel.Name}");
}

Console.WriteLine("\nКонтакты пользователя:");

foreach (var contact in profile.Contacts)
{
    var channel = channels.FirstOrDefault(
        c => c.Id == contact.CommunicationChannelId
    );

    string channelName = channel?.Name ?? "Неизвестный канал";

    Console.WriteLine(
        $"{channelName}: {contact.Value} (ID контакта: {contact.Id})"
    );
}

var (channelCount, contactCount) = (channels.Count, profile.Contacts.Count);

Console.WriteLine("\nСтатистика:");
Console.WriteLine($"Каналов связи: {channelCount}");
Console.WriteLine($"Контактов пользователя: {contactCount}");

Console.WriteLine("\nДемонстрация завершена.");
