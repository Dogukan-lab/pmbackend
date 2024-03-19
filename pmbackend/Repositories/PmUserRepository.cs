using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using pmbackend.Database;
using pmbackend.ErrorTypes;
using pmbackend.Models;

namespace pmbackend;

/**
 * @summary This repository is used to do data manipulation and handle errors before a result gets returned
 * This repository can also have filtering built in depending if that will be needed or not.
 */
public class PmUserRepository : IPmUserRepository
{
    private readonly PaleMessengerContext _context;
    private readonly UserManager<PmUser> _manager;

    public PmUserRepository(PaleMessengerContext paleContext, UserManager<PmUser> manager)
    {
        _context = paleContext;
        _manager = manager;
    }

    /**
     * @brief Gets all Users from the database
     */
    public List<PmUser> GetAllUsers()
    {
        return _manager.Users.ToList();
    }

    /**
     * @brief Gets user by his username
     * Explicitly loads the Friends Collection of specified user.
     */
    public PmUser? GetUser(string username)
    {
        var user = _manager.FindByNameAsync(username).GetAwaiter().GetResult();
        
        if(user is not null) 
            _context.Entry(user).Collection(u => u.Friends!).Load();

        return user;
    }

    /**
     * @brief Gets user by Id
     * @return User that has been specified 
     */
    public PmUser? GetUser(int id)
    {
        return _context.Users.FirstOrDefault(res => res.Id == id);
    }

    /**
     * @brief Updates user through Usermanager
     * @return Depending on the update it either sends NO_ERROR or UNABLE_TO_UPDATE.
     */
    public ErrorType UpdateUser(PmUser updatedUser)
    {
        var result = _manager.UpdateAsync(updatedUser).GetAwaiter().IsCompleted;
        return result ? ErrorType.NO_ERROR : ErrorType.UNABLE_TO_UPDATE;
    }

    /**
     * @brief This function adds the bi-directional link between the user and the target user.
     * @param username -> User that wants to add the target
     * @param targetUsername -> User that will add the bi-directional link.
     * @return Errortype -> Depending on the situation will return either USER_NOT_FOUND or NO_ERROR.
     */
    public ErrorType AddFriend(string username, string targetUsername)
    {
        //Find users in database
        var user = _manager.FindByNameAsync(username).GetAwaiter().GetResult();
        var target = _manager.FindByNameAsync(targetUsername).GetAwaiter().GetResult();

        if (user is null || target is null)
            return ErrorType.USER_NOT_FOUND;
        
        //Adds users between each other.
        _context.Entry(user).Collection(usr => usr.Friends!).Load();
        user.Friends!.Add(target);

        _context.Entry(target).Collection(usr => usr.Friends!).Load();
        target.Friends!.Add(user);
        
        //Have to update the table entry to use it further.
        _manager.UpdateAsync(user).GetAwaiter().GetResult();
        _manager.UpdateAsync(target).GetAwaiter().GetResult();
        
        return ErrorType.NO_ERROR;
    }
    
    /**
     * @brief This function removes the bi-directional link between the user and the target user.
     * @param username -> User that wants to remove the target
     * @param targetUsername -> User that will cut the bi-directional link.
     * @return Errortype -> Depending on the situation will return either USER_NOT_FOUND or NO_ERROR.
     */
    public ErrorType RemoveFriend(string username, string targetUsername)
    {
        //Finds users within the user table.
        var user = _manager.FindByNameAsync(username).GetAwaiter().GetResult();
        var targetUser = _manager.FindByNameAsync(targetUsername).GetAwaiter().GetResult();
        
        if (user is null || targetUser is null)
            return ErrorType.USER_NOT_FOUND;

        //Removes the bi-directional link
        _context.Entry(user).Collection(usr => usr.Friends!).Load();
        user.Friends?.Remove(targetUser);
        
        _context.Entry(targetUser).Collection(usr => usr.Friends!).Load();
        targetUser.Friends?.Remove(user);
        
        //Updates the table entries accordingly
        _manager.UpdateAsync(user).GetAwaiter().GetResult();
        _manager.UpdateAsync(targetUser).GetAwaiter().GetResult();
        return ErrorType.NO_ERROR;
    }
}