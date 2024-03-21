namespace pmbackend.Models.Dto;

public class MessageDto
{
    public string Data { get; set; }
    public DateTime TimeStamp { get; set; }

    public int ChatId { get; set; }
    public int UserId { get; set; }
}