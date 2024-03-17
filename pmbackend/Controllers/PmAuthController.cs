using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using pmbackend.Models;
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

        private readonly IPmUserRepository _userRepository;

        private readonly IMapper _mapper;

        public PmAuthController(IAuthService authService, IMapper mapper,
            IPmUserRepository pmUserRepository)
        {
            _authService = authService;
            _mapper = mapper;
            _userRepository = pmUserRepository;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUser(PmLoginDto pmLogin)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (await _authService.RegisterUser(pmLogin))
            {
                var user =
                    _mapper.Map<PmUserDto>(
                        _authService.GetUser(pmLogin.Username));

                var token = _authService.GenerateTokenString(pmLogin);
                return Ok(new { token, user });
            }

            return BadRequest("User has not been Registered!");
        }

        [HttpPost("Login")]
        public async Task<IActionResult> LoginUser(PmLoginDto pmUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authService.Login(pmUser);
            if (!result)
            {
                return BadRequest("No proper user credentials!");
            }

            var user =
                _mapper.Map<PmUserDto>(_authService.GetUser(pmUser.Username));
            var tokenString = _authService.GenerateTokenString(pmUser);
            return Ok(new { tokenString, user });
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