using pmbackend.Models;

namespace pmbackend;

public interface IPmUserRepository
{
    List<PmUser> GetAllUsers();

    PmUser? GetUser(string username);
    PmUser? GetUser(int id);
}