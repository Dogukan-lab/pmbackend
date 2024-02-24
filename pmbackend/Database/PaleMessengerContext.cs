using Microsoft.EntityFrameworkCore;
using pmbackend.Models;

namespace pmbackend.Database
{
    public class PaleMessengerContext : DbContext
    {
        public PaleMessengerContext(DbContextOptions<PaleMessengerContext> option) : base(option) { }
        public DbSet<PmUser> PmUsers { get; set; }
    }
}
