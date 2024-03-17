using pmbackend.Database;
using pmbackend.Models;

namespace pmbackend;

public class PmUserRepository: IPmUserRepository
{
    private readonly PaleMessengerContext _context;

    public PmUserRepository(PaleMessengerContext paleContext)
    {
        _context = paleContext; 
    }

    public List<PmUser> GetAllUsers()
    {
        return _context.Users.ToList();
    }

    public PmUser? GetUser(string username)
    {
        return _context.Users.FirstOrDefault(res => res.UserName == username);
    }

    public PmUser? GetUser(int id)
    {
        return _context.Users.FirstOrDefault(res => res.Id == id);
    }
}