using System.ComponentModel.DataAnnotations;

namespace pmbackend.Models;

public class Chat
{
    [Key]
    public int MessageId { get; set; } 
    public bool IsVisible { get; set; }
    public virtual ICollection<PmUser>? Users { get; set; }
    public virtual ICollection<Message>? Messages { get; set; }
}