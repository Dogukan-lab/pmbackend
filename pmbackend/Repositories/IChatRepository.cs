using pmbackend.Models;

namespace pmbackend;

public interface IChatRepository
{
    bool AddChat(Chat chat);
    List<Chat> GetAllChats();
    List<Chat> GetChatsForUser(PmUser user);
    Chat GetChat(int id);
    int GetChatId(string username);
    Task<bool> HideChat();
    bool UpdateChat(Chat newChat);
    public bool AddMessageToChat(int id, Message message);
    public bool AddUserToChat(int id, PmUser user);
}
