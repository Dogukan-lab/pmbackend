using pmbackend.Models;

namespace pmbackend;

public interface IMessageRepository
{
    void AddMessage(Message newMessage);
    List<Message> GetAllMessages();
}