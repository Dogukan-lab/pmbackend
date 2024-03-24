using System.Formats.Asn1;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using pmbackend.Database;
using pmbackend.Models;

namespace pmbackend;

/// <summary>
/// Used as a way to put pre-existing data into the database.
/// This is no longer usefull for the PmUser for that already is used through the identity framework.
/// This will however be used to do fun stuff like chat pre-seeding and the sort.
/// </summary>
public class SeedDb
{
    // public static void SeedDatabase(IApplicationBuilder applicationBuilder)
    // {
    //     using var serviceScope = applicationBuilder.ApplicationServices.CreateScope();
    //     var context = serviceScope.ServiceProvider.GetService<PaleMessengerContext>();
    //     context?.Database.EnsureCreated();
    //
    //
    //     var duncan = new PmUser()
    //     {
    //         Username = "Duncan",
    //         Password = "Duncan",
    //     };
    //
    //     var lars = new PmUser()
    //     {
    //         Username = "Lars",
    //         Password = "Lars",
    //     };
    //
    //     context!.AddRange((object)new List<PmUser>()
    //     {
    //         duncan,
    //         lars,
    //     });
    //
    //     context.SaveChanges();
    // }


    public static void SeedUserIdentities(IApplicationBuilder applicationBuilder)
    {
        using var serviceScope = applicationBuilder.ApplicationServices.CreateScope();
        var context = serviceScope.ServiceProvider.GetService<PaleMessengerContext>();

        if (context is null)
            return;

        if (context.Users.Any())
            return;

        //Create manager
        var userManager =
            serviceScope.ServiceProvider.GetRequiredService<UserManager<PmUser>>();

        //Setup users
        var duncan = new PmUser
        {
            UserName = "Duncan",
            Status = "Alive and well!",
            Bio = "I like turtles",
            ProfileIcon = 0,
            Background = 0,
        };
        var lars = new PmUser
        {
            UserName = "Lars",
            Status = "Freetime privileges revoked",
            Bio = "I don't like turtles!",
            ProfileIcon = 0,
            Background = 0,
        };
        var tester = new PmUser
        {
            UserName = "Tester",
            Status = "Currently testing things...",
            Bio = "I love testing!",
            ProfileIcon = 1,
            Background = 0,
        };
        var owen = new PmUser
        {
            UserName = "Owen",
            Status = "Alive and well!",
            Bio = "I like turtles",
            ProfileIcon = 3,
            Background = 1,
        };
        userManager.CreateAsync(duncan, "Duncan#1").GetAwaiter().GetResult();
        userManager.CreateAsync(lars, "Lars#1").GetAwaiter().GetResult();
        userManager.CreateAsync(tester, "Tester#1").GetAwaiter().GetResult();
        userManager.CreateAsync(owen, "Owen#1").GetAwaiter().GetResult();

        //Add friend relationship
        duncan.Friends = new List<PmUser> { lars, tester, };
        lars.Friends = new List<PmUser> { duncan, tester, owen };
        tester.Friends = new List<PmUser> { lars, duncan, };
        owen.Friends = new List<PmUser> { lars, };

        //Update created entries
        userManager.UpdateAsync(duncan).GetAwaiter().GetResult();
        userManager.UpdateAsync(lars).GetAwaiter().GetResult();
        userManager.UpdateAsync(tester).GetAwaiter().GetResult();
        userManager.UpdateAsync(owen).GetAwaiter().GetResult();


        //Create chats
        if (context.Chats.Any())
            return;

        var chatDL = new Chat
        {
            IsVisible = true,
            Messages = new List<Message>
            {
                new Message
                {
                    Data = "Test message one",
                    TimeStamp = DateTime.Now.ToLocalTime().AddHours(-2),
                    User = duncan
                },
                new Message
                {
                    Data = "10? Waarom 10?",
                    TimeStamp = DateTime.Now.ToLocalTime().AddHours(-1),
                    User = lars
                },
                new Message
                {
                    Data = "Da's meer dan 11",
                    TimeStamp = DateTime.Now.ToLocalTime(),
                    User = duncan
                },
            },
            Users = new List<PmUser> { duncan, lars },
        };
        var chatDT = new Chat
        {
            IsVisible = true,
            Messages = new List<Message>
            {
                new Message
                {
                    Data = "Tester messaging Duncan",
                    TimeStamp = DateTime.Now.ToLocalTime().AddHours(-1),
                    User = tester
                },
                new Message
                {
                    Data = "Messaging tester from Duncan",
                    TimeStamp = DateTime.Now.ToLocalTime(),
                    User = duncan
                },
            },
            Users = new List<PmUser> { tester, duncan },
        };

        var chatLO = new Chat
        {
            IsVisible = true,
            Messages = new List<Message>
            {
                new Message
                {
                    Data = "Yo OWEN!",
                    TimeStamp = DateTime.Now.ToLocalTime().AddHours(-3),
                    User = lars
                },
                new Message
                {
                    Data = "Breh",
                    TimeStamp = DateTime.Now.ToLocalTime().AddHours(-2),
                    User = owen
                },
                new Message
                {
                    Data = "No waying you're insane",
                    TimeStamp = DateTime.Now.ToLocalTime().AddHours(-1),
                    User = owen
                },
                new Message
                {
                    Data = "Pyramid.Spawn()",
                    TimeStamp = DateTime.Now.ToLocalTime(),
                    User = owen
                },
            },
            Users = new List<PmUser> { lars, owen },
        };

        var chats = new List<Chat> { chatDL, chatDT, chatLO };
        context.Chats.AddRange(chats);
        context.SaveChanges();
    }

    //Maybe check if we can use this instead.
    private void CreateUser(PmUser user, string role)
    {
    }
}