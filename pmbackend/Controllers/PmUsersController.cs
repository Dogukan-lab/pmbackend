using Microsoft.AspNetCore.Mvc;
using pmbackend.Database;
using pmbackend.Dto;
using pmbackend.Models;

namespace pmbackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PmUsersController : Controller
    {
        private readonly PaleMessengerContext _context;

        public PmUsersController(PaleMessengerContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult LoginRequest(PmUserDto user)
        {
            return _context.PmUsers.FirstOrDefault(compare => user.Username == compare.Username &&
                                                              user.Password == compare.Password) != null
                ? Ok()
                : BadRequest("Incorrect");
        }
    }
}
