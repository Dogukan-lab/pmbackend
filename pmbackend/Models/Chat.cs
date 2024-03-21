using System.ComponentModel.DataAnnotations;

namespace pmbackend.Models;

public class Chat
{
    [Key]
    public int ChatId { get; set; } 
    public bool IsVisible { get; set; }
    public virtual ICollection<PmUser>? Users { get; set; }
    public virtual ICollection<Message>? Messages { get; set; }
}