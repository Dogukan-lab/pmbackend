using System.ComponentModel.DataAnnotations;

namespace pmbackend.Models;

public class Message
{
   [Key]
   public int Id;
   public int UserId { get; set; }
   public int ChatId { get; set; }
   public string Data { get; set; } = string.Empty;
   public DateTime TimeStamp { get; set; }
}