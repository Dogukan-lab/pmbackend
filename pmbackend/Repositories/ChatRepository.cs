using Microsoft.EntityFrameworkCore;
using pmbackend.Database;
using pmbackend.Models;

namespace pmbackend;

public class ChatRepository : IChatRepository
{
    private readonly PaleMessengerContext _messengerContext;

    public ChatRepository(PaleMessengerContext context)
    {
        _messengerContext = context;
    }

    public bool AddChat(Chat chat)
    {
        _messengerContext.Chats.Add(chat);
        return _messengerContext.SaveChangesAsync().GetAwaiter().GetResult() > 0;
    }

    public bool AddUserToChat(int id, PmUser user)
    {
        var chat = _messengerContext.Chats.FirstOrDefault(res => res.ChatId == id);
        if (chat is null)
            return false;
        chat.Users?.Add(user);
        return _messengerContext.SaveChangesAsync().GetAwaiter().GetResult() > 0;
    }

    public bool AddMessageToChat(int id, Message message)
    {
        var chat = _messengerContext.Chats
            .Include(chat => chat.Messages)
            .Include(chat => chat.Users)
            .FirstOrDefault(res => res.ChatId == id);
        chat?.Messages?.Add(message);

        return _messengerContext.SaveChangesAsync().GetAwaiter().GetResult() > 0;
    }

    public List<Chat> GetAllChats()
    {
        return _messengerContext.Chats
            .Include(chats => chats.Users)
            .Include(chats => chats.Messages)
            .ToList();
    }

    public Chat GetChat(int id)
    {
        return _messengerContext.Chats.Include(chats => chats.Users)
            .Include(chats => chats.Messages)
            .FirstOrDefault(res => res.ChatId == id)!;
    }

    public Task<bool> HideChat()
    {
        throw new NotImplementedException();
    }

    public bool UpdateChat(Chat newChat)
    {
        _messengerContext.Chats.Update(newChat);
        return _messengerContext.SaveChangesAsync().GetAwaiter().GetResult() > 0;
    }
}