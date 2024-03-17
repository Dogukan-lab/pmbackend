using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace pmbackend.Models
{
    public class PmUser: IdentityUser<int>
    {
        public override int Id { get; set; }
        public int ProfileIcon { get; set; }
        public int Background { get; set; }
        
        //TODO Figure something out to fix this mess.
        public virtual ICollection<PmUser> Friends { get; set; }
    }
}