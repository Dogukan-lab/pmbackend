using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using pmbackend.Models;

namespace pmbackend.Database
{
    /// <summary>
    /// Database context to do fun stuff with
    /// Used as an access point for the database.
    /// TODO Add complex relations to this alongside of a custom IdentitUser
    /// </summary>
    public class PaleMessengerContext : IdentityDbContext<PmUser, IdentityRole<int>, int>
    {
        public PaleMessengerContext(DbContextOptions<PaleMessengerContext> option) : base(option) { }
    }
}
