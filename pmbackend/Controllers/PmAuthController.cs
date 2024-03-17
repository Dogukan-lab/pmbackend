using Microsoft.AspNetCore.Mvc;
using pmbackend.Models.Dto;

namespace pmbackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PmAuthController : Controller
    {
        /// <summary>
        /// This controller is used for registration and login of a Pale Messenger User
        /// The IAuthService handles registration, login and token generation
        /// </summary>
        private readonly IAuthService _authService;

        public PmAuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUser(PmUserDto pmUserDTO)
        {   
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if(await _authService.RegisterUser(pmUserDTO))
            {
                return Ok($"Register user: {pmUserDTO.Username}");
            }

            return BadRequest("User has not been Registered!");  

        }

        [HttpPost("Login")]
        public async Task<IActionResult> LoginUser(PmUserDto pmUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authService.Login(pmUser);
            if(!result)
            {
                return BadRequest("No proper user credentials!");
            }

            var tokenString = _authService.GenerateTokenString(pmUser);
            return Ok(tokenString);
        }

        //Old Version
        //[HttpPost]
        //public IActionResult LoginRequest(PmUserDto userDTO)
        //{

        //    string passwordHash = BCrypt.Net.BCrypt.HashPassword(userDTO.Password);

        //    var user = new PmUser();

        //    user.Username = userDTO.Username;
        //    user.PasswordHash = passwordHash;

        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    return Ok();


        //    //return _context.PmUsers.FirstOrDefault(compare => user.Username == compare.Username &&
        //    //                                                  user.Password == compare.Password) != null
        //    //    ? Ok()
        //    //    : BadRequest("Incorrect");
        //}

    }
}
