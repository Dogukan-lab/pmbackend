using pmbackend.ErrorTypes;
using pmbackend.Models;
using pmbackend.Models.Dto;

namespace pmbackend
{
    /// <summary>
    /// Interface that is used as an abstraction layer between controllers and the implementation.
    /// </summary>
    public interface IAuthService
    {
        Task<ErrorType> RegisterUser(PmLoginDto pmLogin);
        Task<bool> Login(PmLoginDto pmLogin);
        string GenerateTokenString(PmLoginDto pmUser);

        PmUser GetUser(string userName);

        Task<ErrorType> DeleteUser(string claim, string username);
    }
}