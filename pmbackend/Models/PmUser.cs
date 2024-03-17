using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace pmbackend.Models
{
    public class PmUser: IdentityUser<int>
    {
        public override int Id { get; set; }
        public int ProfileIcon { get; set; } = 0;
        public int Background { get; set; } = 0;
        public string Status { get; set; } = string.Empty;
        public bool IsOnline { get; set; } = true;
        
        //TODO Figure something out to fix this mess.
        public virtual ICollection<PmUser>? Friends { get; set; }
    }
}