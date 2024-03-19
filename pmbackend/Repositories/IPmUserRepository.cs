using pmbackend.ErrorTypes;
using pmbackend.Models;

namespace pmbackend;

public interface IPmUserRepository
{
    List<PmUser> GetAllUsers();

    PmUser? GetUser(string username);
    PmUser? GetUser(int id);
    
    ErrorType UpdateUser(PmUser updatedUser);

    ErrorType AddFriend(string username, string targetUsername);

    ErrorType RemoveFriend(string username, string targetUsername);
}