using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
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
        public PaleMessengerContext(DbContextOptions<PaleMessengerContext> option) : base(option)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PmUser>(user =>
            {
                user.HasMany(f => f.Friends)
                    .WithMany()
                    .UsingEntity<Dictionary<string, object>>("UserFriend",
                        j => j.HasOne<PmUser>().WithMany(),
                        j => j.HasOne<PmUser>().WithMany()
                    );
            });
        }
    }
}