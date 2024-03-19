using System.ComponentModel.DataAnnotations;

namespace pmbackend.Models;

public class Chat
{
    [Key]
    public int Id; 
    public bool IsVisible { get; set; }
    public ICollection<PmUser>? Users { get; set; }
    public ICollection<Message>? Messages { get; set; }
}