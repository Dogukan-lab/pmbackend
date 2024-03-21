using pmbackend.Models;

namespace pmbackend;

public interface IChatRepository
{
    bool AddChat(Chat chat);
    List<Chat> GetAllChats();
    Chat GetChat(int id);
    Task<bool> HideChat();
    bool UpdateChat(Chat newChat);
    public bool AddMessageToChat(int id, Message message);
    public bool AddUserToChat(int id, PmUser user);
}
