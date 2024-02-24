using System.ComponentModel.DataAnnotations;

namespace pmbackend.Models
{
    public class PmUser
    {
        [Key]
        public int UserId { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
    }
}
