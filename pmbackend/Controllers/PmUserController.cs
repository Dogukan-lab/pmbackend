using Microsoft.AspNetCore.Mvc;

namespace pmbackend.Controllers
{
    public class PmUserController: Controller
    {
        private readonly IPmUserRepository _userRepository;
    }
}