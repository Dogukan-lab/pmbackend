using pmbackend.Database;
using pmbackend.Models;

namespace pmbackend;

public class SeedDb
{
    public static void SeedDatabase(IApplicationBuilder applicationBuilder)
    {
        using var serviceScope = applicationBuilder.ApplicationServices.CreateScope();
        var context = serviceScope.ServiceProvider.GetService<PaleMessengerContext>();
        context?.Database.EnsureCreated();

        if (context!.PmUsers.Any()) return;

        var duncan = new PmUser()
        {
            Username = "Duncan",
            Password = "Duncan",
        };

        var lars = new PmUser()
        {
            Username = "Lars",
            Password = "Lars",
        };

        context.AddRange(new List<PmUser>()
        {
            duncan,
            lars,
        });

        context.SaveChanges();
    }
}