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
        {
            //Maybe return an error instead of a raw return.
            return;
        }

        if (context.Users.Any())
        {
            //Same here.
            return;
        }
        
        //Create manager
        var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<PmUser>>();
            
        //Setup users
        var duncan = new PmUser
        {
            UserName = "Duncan",
            ProfileIcon = 0,
            Background = 0,
        };
        var lars = new PmUser
        {
            UserName = "Lars",
            ProfileIcon = 0,
            Background = 0,
        };
            
        userManager.CreateAsync(duncan, "Duncan#1").GetAwaiter().GetResult();
        userManager.CreateAsync(lars, "Lars#1").GetAwaiter().GetResult();

        // var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        //This doesn't work for some reason
        // var role = new IdentityRole("User");
        // roleManager.CreateAsync(role).GetAwaiter().GetResult();
        // userManager.AddToRoleAsync(duncan, "User").GetAwaiter().GetResult();
        // userManager.AddToRoleAsync(lars, "User").GetAwaiter().GetResult();
    }

    //Maybe check if we can use this instead.
    private void CreateUser(PmUser user, string role)
    {
        
    }
}