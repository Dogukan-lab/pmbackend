namespace pmbackend.Models.Dto;

public class ChatDto
{
    public bool IsVisible { get; set; }
    public ICollection<PmUserDto>? Users { get; set; }
    public ICollection<MessageDto>? Messages { get; set; } 
}