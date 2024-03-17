using pmbackend.Models;
using pmbackend.Models.Dto;

namespace pmbackend
{
    /// <summary>
    /// Interface that is used as an abstraction layer between controllers and the implementation.
    /// </summary>
    public interface IAuthService
    {
        Task<bool> Login(PmUserDto pmUser);
        Task<bool> RegisterUser(PmUserDto pmUser);
        string GenerateTokenString(PmUserDto pmUser);
        Task<ErrorTypes.ErrorType> DeleteUser(string claim, string username);
    }
}