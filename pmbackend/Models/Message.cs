using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pmbackend.Models;

public class Message
{
   [Key] public int MessageId { get; set; }
   
   [ForeignKey("PmUser")]
   public int UserId { get; set; }
   [ForeignKey("Chat")]
   public int ChatId { get; set; }
   public string Data { get; set; } = string.Empty;
   public DateTime TimeStamp { get; set; }
   
   public virtual Chat? Chat { get; set; }
   public virtual PmUser? User { get; set; }
}