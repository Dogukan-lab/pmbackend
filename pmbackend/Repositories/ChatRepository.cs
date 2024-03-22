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

    /**
     * @brief Adds a chat to the database
     * @param chat, the new chat that's to be added.
     */
    public bool AddChat(Chat chat)
    {
        _messengerContext.Chats.Add(chat);
        return _messengerContext.SaveChangesAsync().GetAwaiter().GetResult() > 0;
    }

    /**
     * @brief Adds a user to the associated chat.
     * @param id, the id of the chat
     * @param user, the user to be added, to the chat.
     */
    public bool AddUserToChat(int id, PmUser user)
    {
        var chat = _messengerContext.Chats.FirstOrDefault(res => res.ChatId == id);
        if (chat is null)
            return false;
        chat.Users?.Add(user);
        return _messengerContext.SaveChangesAsync().GetAwaiter().GetResult() > 0;
    }

    /**
     * @brief Checks the database for an existing chat and appends the message to the chat.
     * If the chat does not exists, a new chat will be created instead.
     * @param message, the message to be appended to the found chat.
     * @return false if no chat exists.
     */
    public bool AddMessageToChat(string username, string targetUser, Message message)
    {
        var chat = _messengerContext.Chats
            .Include(chat => chat.Users)
            .Include(chat => chat.Messages)
            .FirstOrDefault(res =>
                res.Users!.Any(user => user.UserName == username)
                && res.Users!.Any(user => user.UserName == targetUser)
            );

        if (chat is null)
            return false;

        chat.Messages?.Add(message);

        return _messengerContext.SaveChangesAsync().GetAwaiter().GetResult() > 0;
    }

    /**
     * @brief Gets all of the chats inside the database.
     * @return List of all chats
     */
    public List<Chat> GetAllChats()
    {
        return _messengerContext.Chats
            .Include(chats => chats.Users)
            .Include(chats => chats.Messages)
            .ToList();
    }

    /**
     * @brief Gets all the chats for a specified user, sorted by newest date time.
     * @param user, user to parse all the chats for.
     * @return List of all chats associated with the user.
     */
    public List<Chat> GetChatsForUser(PmUser user)
    {
        return _messengerContext.Chats.Include(chat => chat.Users)
            .Include(chat =>
                chat.Messages!.OrderByDescending(message => message.TimeStamp))
            .Where(chat => chat.Users!.Any(usr => usr.Id == user.Id)).ToList();
    }

    public Chat GetChat(int id)
    {
        return _messengerContext.Chats.Include(chats => chats.Users)
            .Include(chats => chats.Messages)
            .FirstOrDefault(res => res.ChatId == id)!;
    }
   
    /**
     * @Note Deprecated function, will no longer be used.
     */
    public int GetChatId(string username)
    {
        throw new NotImplementedException();
        // return _messengerContext.Chats
        //     .Include(chat => chat.Users)
        //     .Include(chat => chat.Messages)
        //     .Where(res => res.Users.Contains(username) && res.Users.Contains(secondUser));
    }

    /**
     * @brief Hides the chat specified.
     * @return if the changes have been saved then.
     */
    public Task<bool> HideChat()
    {
        throw new NotImplementedException();
    }

    /**
     * @brief Updates the chat inside of the database
     * @param newChat the model with the updated values to be passed into the database.
     */
    public bool UpdateChat(Chat newChat)
    {
        _messengerContext.Chats.Update(newChat);
        return _messengerContext.SaveChangesAsync().GetAwaiter().GetResult() > 0;
    }
}