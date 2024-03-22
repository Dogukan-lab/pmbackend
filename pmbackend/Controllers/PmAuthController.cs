using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using pmbackend.ErrorTypes;
using pmbackend.Models;
using pmbackend.Models.Dto;

namespace pmbackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //TODO Create seperate controller for user interaction.
    public class PmAuthController : Controller
    {
        /// <summary>
        /// This controller is used for registration and login of a Pale Messenger User
        /// The IAuthService handles registration, login and token generation
        /// </summary>
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;

        public PmAuthController(IAuthService authService, IMapper mapper)
        {
            _authService = authService;
            _mapper = mapper;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUser(PmLoginDto pmLogin)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            switch (await _authService.RegisterUser(pmLogin))
            {
                case ErrorType.VALID_USER:
                    var user =
                        _mapper.Map<PmUserDto>(
                            _authService.GetUser(pmLogin.Username));

                    var token = _authService.GenerateTokenString(pmLogin.Username);
                    return Ok(new { token, user });

                case ErrorType.USERNAME_INVALID_LENGTH:
                    return BadRequest(
                        "Username is shorter than the required length of 4 characters!");

                default:
                    return BadRequest("Unable to register user!");
            }
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
            var token = _authService.GenerateTokenString(pmUser.Username);
            return Ok(new { token, user });
        }
    }
}